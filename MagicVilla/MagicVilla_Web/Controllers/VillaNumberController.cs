﻿using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.ViewModel;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
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
        var response = await _villaNumberService.GetAllAsync<ApiResponse>(GetToken());
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            
        }
        return View(list);
    }

    //For clear form Create VillaNumber witch SelectList (vills)
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateVillaNumber()
    {
        VillaNumberCreateVM villaNumberVm = new();
        await FillVillaList(villaNumberVm);
        return View(villaNumberVm);
    }
    
    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.CreateAsync<ApiResponse>(model.VillaNumber, GetToken());
            return await RedirectIndexVillaNumber(response, model);
        }
        //Update List Vills before uncorrect write Data (NumberVilla Exsist in Villa)
        await FillVillaList(model);
        return View(model);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateVillaNumber(int villaNo)
    {
        VillaNumberUpdateVM villaNumberVm = new();
        var response = await _villaNumberService.GetAsync<ApiResponse>(villaNo, GetToken());
        
        if (response != null && response.IsSuccess)
        {
            VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            villaNumberVm.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(model);
        }
        
        await FillVillaList(villaNumberVm);
        return View(villaNumberVm);
    }
    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.UpdateAsync<ApiResponse>(model.VillaNumber,GetToken());
            return await RedirectIndexVillaNumber(response, model);
        }
        await FillVillaList(model);
        return View(model);
    }
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteVillaNumber(int villaNo)
    {
        VillaNumberDeleteVM villaNumberVm = new();
        var response = await _villaNumberService.GetAsync<ApiResponse>(villaNo, GetToken());
        
        if (response != null && response.IsSuccess)
        {
            VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            villaNumberVm.VillaNumber = model;
        }
        
        await FillVillaList(villaNumberVm);
        return View(villaNumberVm);
    }
    
    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
    {
        var response = await _villaNumberService.DeleteAsync<ApiResponse>(model.VillaNumber.VillaNo, GetToken());
        return await RedirectIndexVillaNumber(response,model);
    }

    //Fill Select List Vills in VillNumber
    private async Task FillVillaList <T> (T villaNumberVm) where T: VillaNumberVM
    {
        var response = await _villaService.GetAllAsync<ApiResponse>(GetToken());
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
    
    private async Task<IActionResult> RedirectIndexVillaNumber <T>(ApiResponse response, T model)
    {
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Current Action with the villa was completed successfully";
            return RedirectToAction(nameof(IndexVillaNumber));
        }
        else
        {
            if (response.ErrorMessages.Count > 0)
            {
                ModelState.AddModelError("ErrorMessages",response.ErrorMessages.FirstOrDefault());
            }
        }
        return View(model);
    }
    
    private string GetToken()
    {
        return HttpContext.Session.GetString(SD.SessionToken);
    }
}