using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModel;

public class VillaNumberDeleteVM: VillaNumberVM
{
    public VillaNumberDeleteVM()
    {
        VillaNumber = new VillaNumberDTO();
    }
    public VillaNumberDTO VillaNumber { get; set; }
}