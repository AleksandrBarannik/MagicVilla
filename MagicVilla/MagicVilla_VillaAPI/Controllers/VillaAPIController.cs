using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

//Устанавливаем маршрут
[Route("api/VillaAPI")]
[ApiController]
public class VillaAPIController: ControllerBase
{
    //Задаем endPoint
    
    [HttpGet]
    public IEnumerable<VillaDTO> GetVillas()
    {
        return VillaStore.villaList;
    }
}