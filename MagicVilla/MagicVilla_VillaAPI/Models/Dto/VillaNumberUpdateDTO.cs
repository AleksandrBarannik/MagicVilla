using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dto;

public class VillaNumberUpdateDTO
{
    [Required]
    public int VillaNo { get; set; }
    public int VillaID { get; set; }//Foreign key to villa
    public string SpecialDetails { get; set; }
}