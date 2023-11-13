using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.ViewModel;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaNumberController:Controller
{
    private readonly IVillaNumberService _villaNumberService;
    private readonly IMapper _mapper;
    private readonly IVillaService _villaService;
    public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper,IVillaService villaService)
    {
        _villaNumberService = villaNumberService;
        _mapper = mapper;
        _villaService = villaService;
    }
    
    public async Task<IActionResult> IndexVillaNumber()
    {
        List<VillaNumberDTO> list = new();
        var response = await _villaNumberService.GetAllAsync<ApiResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            
        }
        return View(list);
    }
    
    //For clear form Create VillaNumber witch SelectList (vills)
    public async Task<IActionResult> CreateVillaNumber()
    {
        VillaNumberCreateVM villaNumberVm = new();
        var response = await _villaService.GetAllAsync<ApiResponse>();
        if (response != null && response.IsSuccess)
        {
            villaNumberVm.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                (Convert.ToString(response.Result)).Select(i=> new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
        }
        return View(villaNumberVm);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.CreateAsync<ApiResponse>(model.VillaNumber);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            else
            {
                if (response.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessages",response.ErrorMessages.FirstOrDefault());
                }
            }
        }
        
        //Update List Vills before uncorrect write Data (Номер Виллы уже существует in другой Вилле)
        var newResponse = await _villaService.GetAllAsync<ApiResponse>();
        if (newResponse != null && newResponse.IsSuccess)
        {
           model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                (Convert.ToString(newResponse.Result)).Select(i=> new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }
        return View(model);
    }

    
    
    
    public async Task<IActionResult> UpdateVillaNumber(int villaId)
    {
        var response = await _villaNumberService.GetAsync<ApiResponse>(villaId);
        if (response != null && response.IsSuccess)
        {
            VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
            return View(_mapper.Map<VillaUpdateDTO>(model));
        } 
        return NotFound();
    }
    
    public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateDTO model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.UpdateAsync<ApiResponse>(model);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
        }
        return NotFound();
    }
    
    public async Task<IActionResult> DeleteVilla(int villaId)
    {
        var response = await _villaNumberService.GetAsync<ApiResponse>(villaId);

        if (response != null && response.IsSuccess)
        {
            VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
            return View(model);
        }
        return NotFound();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVillaNumber(VillaNumberDTO model)
    {
        var response = await _villaNumberService.DeleteAsync<ApiResponse>(model.VillaNo);
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(IndexVillaNumber));
        }
        return View(model);
    }
}