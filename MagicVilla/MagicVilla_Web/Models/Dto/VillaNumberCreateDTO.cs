﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto;

public class VillaNumberCreateDTO
{
    [Required]
    public int VillaNo { get; set; }
    [Required]
    public int VillaID { get; set; }//Foreign key to villa
    public string SpecialDetails { get; set; }
}