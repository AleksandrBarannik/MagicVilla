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
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
        return Ok(VillaStore.villaList);
    }
    
    [HttpGet("{id:int}")]
    public ActionResult<VillaDTO> GetVilla(int id)
    {//С помощью ActionResult определяем тип возвращаемого значения
        return Ok(VillaStore.villaList.FirstOrDefault(u=>u.Id==id));
    }
    
}