using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

//Устанавливаем маршрут
[Route("api/VillaAPI")]
[ApiController]
public class VillaAPIController: ControllerBase
{
    //Задаем endPoint
    
    [HttpGet]
    public IEnumerable<Villa> GetVillas()
    {
        return new List<Villa>
        {
            new Villa { Id = 1, Name = "Pool View" },
            new Villa { Id = 2, Name = "Beach View" }
        };
    }
}