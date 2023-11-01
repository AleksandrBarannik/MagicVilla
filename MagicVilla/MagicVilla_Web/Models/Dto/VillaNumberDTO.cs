using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto;

public class VillaNumberDTO
{
    [Required]
    public int VillaNo { get; set; }
    public int VillaID { get; set; }//Foreign key to villa
    public string SpecialDetails { get; set; }
}