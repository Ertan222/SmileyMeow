using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalAPI.Models.AnimalInformationn;
using AnimalAPI.Models.Breedd;
using AnimalAPI.Models.Speciee;

namespace AnimalAPI.Models.Animall;

public class Animal : IAnimal
{
    public int AnimalId { get ; set ; }
    public int SpecieId { get ; set ; }
    public Specie Specie { get; set; }
    public int BreedId { get ; set ; }
    public Breed Breed { get; set; }
    public int AnimalInfoId { get; set; }
    public AnimalInfo AnimalInfo { get; set; }
}
