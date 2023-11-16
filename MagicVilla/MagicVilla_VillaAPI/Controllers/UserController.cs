using System.Net;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto.Identity;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

[Route("api/UsersAuth")]
[ApiController]
public class UserController:Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ApiResponse _response;
    private string _message;

    public UserController(IUserRepository userRepository)
    {
        this._response = new();
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
    {
        var loginResponse = await _userRepository.Login(model);
        if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
        {
            _message = "UserName or Password Incorrect";
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add(_message);
            return BadRequest(_response);
        }
        _response.Result = loginResponse;
        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        return Ok(_response);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
    {
        bool userNameUnique = _userRepository.IsUniqueUser(model.UserName);
        if (!userNameUnique)
        {
            _message = "UserName already Exsist";
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add(_message);
            return BadRequest(_response);
        }
        var user = await _userRepository.Register(model);
        if (user == null)
        {
            _message = "Error While registering";
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add(_message);
            return BadRequest(_response);
        }
        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        return Ok(_response);
    }
}