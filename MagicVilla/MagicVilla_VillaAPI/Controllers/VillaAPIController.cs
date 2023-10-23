﻿using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

//Устанавливаем маршрут
[Route("api/VillaAPI")]
[ApiController]
public class VillaApiController: ControllerBase
{
    //Задаем endPoint
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
        return Ok(VillaStore.villaList);
    }
    
    [HttpGet("{id:int}",Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound )]
    public ActionResult<VillaDTO> GetVilla(int id)
    {//С помощью ActionResult определяем тип возвращаемого значения
        if (id == 0)
        {
            return BadRequest();
        }
        var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
        
        if (villa == null)
        {
            return NotFound();
        }
        return Ok(villa);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError )]
    public ActionResult<VillaDTO>CreateVilla([FromBody]VillaDTO? villaDto)
    {
        if (villaDto == null)
        {
            return BadRequest();
        }

        if (villaDto.Id > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        //Сортируем по убыванию и берем первый элемент
        villaDto.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault()!.Id + 1;
        VillaStore.villaList.Add(villaDto);
        
        return CreatedAtRoute("GetVilla",new {id = villaDto.Id},villaDto);
    }
    
}