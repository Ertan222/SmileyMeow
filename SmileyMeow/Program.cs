using System;
using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmileyMeow.Data;
using SmileyMeow.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorOptions(
    page =>
    {
        page.ViewLocationFormats.Add("/Views/Admin/{1}/{0}.cshtml");
    }
);

builder.Services.AddDbContext<SmileyMeowDbContext>(
        pgsql => pgsql.UseNpgsql(builder.Configuration.GetConnectionString("SmileyPSQLConnection"))
        .UseLowerCaseNamingConvention()
);
// Hangfire//
builder.Services.AddHangfire(opt =>
        opt.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSQLiteStorage(builder.Configuration.GetConnectionString("HangfireConnection")
        ));

builder.Services.AddHangfireServer();
// builder.Services.AddScoped<IWriteToFileService,WriteToFileService>();
builder.Services.AddScoped<IUpdateAppointmentService,UpdateAppointmentService>();
//

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddTransient<IRandomAnimalService, RandomAnimalService>();

builder.Services.AddHttpClient();

builder.Services.Configure<CookieTempDataProviderOptions>(
    opt =>
    {
        opt.Cookie.IsEssential = true;
        opt.Cookie.HttpOnly = true;
        opt.Cookie.Name = "AuthCookie";
    }
);

builder.Services.Configure<CookiePolicyOptions>(
    opt =>
    {
        opt.CheckConsentNeeded = context => true;
        opt.MinimumSameSitePolicy = SameSiteMode.None;
    }
);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
      .AddCookie(
          opt =>
          {
              opt.AccessDeniedPath = "/Account/Login";
              opt.LoginPath = "/Account/Login";
              opt.LogoutPath = "/Account/Logout";
          }

);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseStaticFiles();

app.UseHangfireDashboard();
BackgroundJob.Enqueue(() => Console.WriteLine("Hangfire service started"));
RecurringJob.AddOrUpdate<IUpdateAppointmentService>(
    "updateappointments",
    x => x.GetAppointmentsFromDb(),
    "*/2 * * * *"
);
app.UseRouting();

app.UseCookiePolicy();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
