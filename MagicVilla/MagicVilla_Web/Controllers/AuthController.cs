using System.Security.Claims;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.Dto.Identity;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        LoginRequestDTO loginRequest = new();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequestDTO loginRequest)
    {
        ApiResponse response = await _authService.LoginAsync<ApiResponse>(loginRequest);
        if (response != null && response.IsSuccess)
        {
            LoginResponseDTO model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

            //Для запоминания что мы  залогинились
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, model.User.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, model.User.Role));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            
            HttpContext.Session.SetString(SD.SessionToken,model.Token);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("CustomError",response.ErrorMessages.FirstOrDefault());
            return View(loginRequest);
        }
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegistrationRequestDTO registrRequest)
    {
        ApiResponse response = await _authService.RegisterAsync<ApiResponse>(registrRequest);
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction("Login");
        }
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        HttpContext.Session.SetString(SD.SessionToken,"");
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public IActionResult AccessDefined()
    {
        return View();
    }
    

}