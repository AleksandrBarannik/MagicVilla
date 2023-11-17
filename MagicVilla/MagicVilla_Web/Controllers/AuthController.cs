﻿using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto.Identity;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_Web.Controllers;

public class AuthController:Controller
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        LoginRequestDTO obj = new();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequestDTO obj)
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegistrationRequestDTO obj)
    {
        ApiResponse response = await _authService.RegisterAsync<ApiResponse>(obj);
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction("Login");
        }
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult AccessDefined()
    {
        return View();
    }
    

}