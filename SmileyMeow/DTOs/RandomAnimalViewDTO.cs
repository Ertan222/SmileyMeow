using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmileyMeow.DTOs;

public record RandomAnimalViewDTO(
    string SName,
    string BName,
    string AnimalInformation
);
