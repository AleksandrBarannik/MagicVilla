using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

//Устанавливаем маршрут 
[Route("api/VillaAPI")]
[ApiController]
public class VillaApiController: ControllerBase
{
    private readonly Ilogging _logger;
    public VillaApiController(Ilogging logger)
    {
        _logger = logger;
    }
    
    //Задаем endPoint
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
        _logger.Log("Getting all villas","");
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
            _logger.Log("Get Villa Error with Id= " + id ,"error");
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
        if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDto?.Name.ToLower()) != null)
        {
            ModelState.AddModelError("CustomError","Villa already Exsist");
            return BadRequest(ModelState);
        }
        if (villaDto == null)
        {
            return BadRequest();
        }

        if (villaDto.Id > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
        
        villaDto.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault()!.Id + 1;
        VillaStore.villaList.Add(villaDto);
        
        return CreatedAtRoute("GetVilla",new {id = villaDto.Id},villaDto);
    }
    
    
    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteVilla(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);

        if (villa == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        VillaStore.villaList.Remove(villa);
        return NoContent();
    }

    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    
    public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDto)
    {
        if (villaDto == null || id != villaDto.Id)
        {
            return BadRequest();
        }

        var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
        villa.Name = villaDto.Name;
        villa.Sqft = villaDto.Sqft;
        villa.Occupancy = villaDto.Occupancy;
        villa.Price = villaDto.Price;
        return NoContent();

    }

    [HttpPatch("{id:int}", Name = "UpdateParticalVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]

    public IActionResult UpdateParticalVilla(int id, JsonPatchDocument<VillaDTO> patchDto)
    {
        if (patchDto == null || id == 0)
        {
            return BadRequest();
        }

        var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
        if (villa == null)
        {
            return BadRequest();
        }
        patchDto.ApplyTo(villa,ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        return NoContent();
    }

}