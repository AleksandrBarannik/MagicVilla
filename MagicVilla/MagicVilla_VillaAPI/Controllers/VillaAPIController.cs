using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVillaVillaAPI.Migrations;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

//Set route
[Route("api/VillaAPI")]
[ApiController]
public class VillaApiController: ControllerBase
{
    private readonly ILogger _logger;
    private readonly ApplicationDbContext _db;
    public VillaApiController(ILogger logger,ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }
    
    //Set endPoint
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
        _logger.LogInformation("Getting all villas");
        return Ok(_db.Villas.ToList());
    }
    
    [HttpGet("{id:int}",Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound )]
    public ActionResult<VillaDTO> GetVilla(int id)
    {//С помощью ActionResult определяем тип возвращаемого значения
        if (id == 0)
        {
            _logger.LogError("Get Villa Error with Id= " + id);
            return BadRequest();
        }
        var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
        
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
    public ActionResult<VillaDTO>CreateVilla([FromBody]VillaDTO villaDto)
    {
        if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDto.Name.ToLower()) != null)
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
        //Convert types VillaDto to Villa
        Villa model = new Villa()
        {
            Id = villaDto.Id,
            Name = villaDto.Name,
            Details = villaDto.Details,
            Rate = villaDto.Rate,
            Occupancy = villaDto.Occupancy,
            Sqft = villaDto.Sqft,
            ImageUrl = villaDto.ImageUrl
        };
        
        _db.Villas.Add(model);
        _db.SaveChanges();
        
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

        var villa =_db.Villas.FirstOrDefault(u => u.Id == id);

        if (villa == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        _db.Villas.Remove(villa);
        _db.SaveChanges();
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
        
        //Convert types VillaDto to Villa
        Villa model = new ()
        {
            Id = villaDto.Id,
            Name = villaDto.Name,
            Details = villaDto.Details,
            Rate = villaDto.Rate,
            Occupancy = villaDto.Occupancy,
            Sqft = villaDto.Sqft,
            ImageUrl = villaDto.ImageUrl
        };
        _db.Villas.Update(model);
        _db.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id:int}", Name = "UpdateParticalVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]

    public IActionResult UpdateParticalVilla(int id, JsonPatchDocument<VillaDTO>? patchDto)
    {
        if (patchDto == null || id == 0)
        {
            return BadRequest();
        }
        var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
        
        //Convert types Villa to VillaDto
        VillaDTO villaDto = new ()
        {
            Id = villa.Id,
            Name = villa.Name,
            Details = villa.Details,
            Rate = villa.Rate,
            Occupancy = villa.Occupancy,
            Sqft = villa.Sqft,
            ImageUrl = villa.ImageUrl
        };

        if (villa == null)
        {
            return BadRequest();
        }
        
        patchDto.ApplyTo(villaDto,ModelState);
        
        // Convert types VillaDto to Villa
        Villa model = new Villa()
        {
            Id = villaDto.Id,
            Name = villaDto.Name,
            Details = villaDto.Details,
            Rate = villaDto.Rate,
            Occupancy = villaDto.Occupancy,
            Sqft = villaDto.Sqft,
            ImageUrl = villaDto.ImageUrl
        };
        _db.Villas.Update(model);
        _db.SaveChanges();
        
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        return NoContent();
    }

}