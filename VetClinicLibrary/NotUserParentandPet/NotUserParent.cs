using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinicLibrary.Person.Locationn;

namespace VetClinicLibrary.NotUserParentandPet;

public class NotUserParent : INotUserParent
{
    public int NotUserParentId { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public int AddressId { get; set; }
    public Address Address { get; set; }
    public List<NotUserParentnPet> NotUsersParentsPets { get; set; }
}
