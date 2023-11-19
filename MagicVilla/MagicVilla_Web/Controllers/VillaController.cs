using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaController:Controller
{
    private readonly IVillaService _villaService;
    private readonly IMapper _mapper;
    
    public VillaController(IVillaService villaService, IMapper mapper)
    {
        _villaService = villaService;
        _mapper = mapper;
    }
    public async Task<IActionResult> IndexVilla()
    {
        List<VillaDTO> list = new();
        var response = await _villaService.GetAllAsync<ApiResponse>(GetToken());
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            
        }
        return View(list);
    }
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateVilla()
    {
        return  View();
    }
    
    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaService.CreateAsync<ApiResponse>(model, GetToken());
            return await RedirectVillaIndex(response);
        }
        TempData["error"] = "Error encountered";
        return View(model);
    }
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateVilla(int villaId)
    {
        var response = await _villaService.GetAsync<ApiResponse>(villaId, GetToken());
        return await CheckResponse<VillaUpdateDTO>(response);
    }
    
    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaService.UpdateAsync<ApiResponse>(model, GetToken());
            return await RedirectVillaIndex(response);
        }
        TempData["error"] = "Error encountered";
        return View(model);
    }
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteVilla(int villaId)
    {
        var response = await _villaService.GetAsync<ApiResponse>(villaId, GetToken());
        return await CheckResponse<VillaDTO>(response);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVilla(VillaDTO model)
    {
        var response = await _villaService.DeleteAsync<ApiResponse>(model.Id, GetToken());
        return await RedirectVillaIndex(response);
    }
    
    private async Task<IActionResult> CheckResponse <T>(ApiResponse response)
    {
        if (response != null && response.IsSuccess)
        {
            VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
            return View(_mapper.Map<T>(model));
        }
        return NotFound();
    }

    private async Task<IActionResult> RedirectVillaIndex(ApiResponse response)
    {
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Current Action with the villa was completed successfully";
            return RedirectToAction(nameof(IndexVilla));
        }
        else
        {
            if (response.ErrorMessages.Count > 0)
            {
                ModelState.AddModelError("ErrorMessages",response.ErrorMessages.FirstOrDefault());
            }
        }
        return View();
    }
    
    private string GetToken()
    {
       return HttpContext.Session.GetString(SD.SessionToken);
    }
    
    
}