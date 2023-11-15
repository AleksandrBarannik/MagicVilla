using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModel;

public abstract class VillaNumberVM
{
    [ValidateNever]
    public IEnumerable<SelectListItem>VillaList { get; set; }
}