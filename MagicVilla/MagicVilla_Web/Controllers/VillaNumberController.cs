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
        await FillVillaList(villaNumberVm);
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
        await FillVillaList(model);
        return View(model);
    }

    public async Task<IActionResult> UpdateVillaNumber(int villaNo)
    {
        VillaNumberUpdateVM villaNumberVm = new();
        var response = await _villaNumberService.GetAsync<ApiResponse>(villaNo);
        if (response != null && response.IsSuccess)
        {
            VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            villaNumberVm.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(model);
        }
        await FillVillaList(villaNumberVm);
        return View(villaNumberVm);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.UpdateAsync<ApiResponse>(model.VillaNumber);
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
        await FillVillaList(model);
        return View(model);
    }
    
    public async Task<IActionResult> DeleteVillaNumber(int villaNo)
    {
        VillaNumberDeleteVM villaNumberVm = new();
        var response = await _villaNumberService.GetAsync<ApiResponse>(villaNo);
        if (response != null && response.IsSuccess)
        {
            VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            villaNumberVm.VillaNumber = _mapper.Map<VillaNumberDTO>(model);
        }
        await FillVillaList(villaNumberVm);
        return View(villaNumberVm);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
    {
        var response = await _villaNumberService.DeleteAsync<ApiResponse>(model.VillaNumber.VillaNo);
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(IndexVillaNumber));
        }
        return View(model);
    }

    //Заполнение Выпадающего списка вилл (для создания Номеров Вилл)
    public async Task FillVillaList(VillaNumberCreateVM villaNumberVm)
    {
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
    }
    public async Task FillVillaList(VillaNumberUpdateVM villaNumberVm)
    {
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
    }
    public async Task FillVillaList(VillaNumberDeleteVM villaNumberVm)
    {
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
    }
}