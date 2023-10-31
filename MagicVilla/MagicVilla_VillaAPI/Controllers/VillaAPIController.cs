using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using MagicVillaVillaAPI.Migrations;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers;

//Set route
[Route("api/VillaAPI")]
[ApiController]
public class VillaApiController: ControllerBase
{
    private readonly IVillaRepository _dbVilla;
    private readonly IMapper _mapper;
    public VillaApiController(IVillaRepository dbVilla,IMapper mapper)
    {
        _dbVilla = dbVilla;
        _mapper = mapper;
    }
    
    
    [HttpGet] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async  Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
    {
        IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
        return Ok(_mapper.Map<List<VillaDTO>>(villaList));
    }
    
    [HttpGet("{id:int}",Name = "GetVilla")] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound )]
    public async Task<ActionResult<VillaDTO>> GetVilla(int id)
    {//С помощью ActionResult определяем тип возвращаемого значения
        if (id == 0)
        {
            return BadRequest();
        }
        var villa = await _dbVilla.GetAsync(u => u.Id == id);
        
        if (villa == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<VillaDTO>(villa));
    }

    [HttpPost] 
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError )]
    public async Task<ActionResult<VillaDTO>>CreateVilla([FromBody]VillaCreateDTO createDto)
    {
        if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDto.Name.ToLower()) != null)
        {
            ModelState.AddModelError("CustomError","Villa already Exsist");
            return BadRequest(ModelState);
        }
        if (createDto == null)
        {
            return BadRequest(createDto);
        }

        //Convert types VillaDto to Villa (using AutoMapper(use MapperConfig))
        Villa model = _mapper.Map<Villa>(createDto);

        await _dbVilla.CreateAsync(model);
        return CreatedAtRoute("GetVilla",new {id = model.Id},model);
    }
    
    
    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult >DeleteVilla(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var villa = await _dbVilla.GetAsync(u => u.Id == id);

        if (villa == null)
        {
            return NotFound();
        }

        await _dbVilla.RemoveAsync(villa);
        return NoContent();
    }

    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    
    public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDto)
    {
        if (updateDto == null || id != updateDto.Id)
        {
            return BadRequest();
        }
        
        //Convert types VillaDto to Villa (using AutoMapper(use MapperConfig))
        Villa model = _mapper.Map<Villa>(updateDto);

        await _dbVilla.UpdateAsync(model);
        return NoContent();
    }

    [HttpPatch("{id:int}", Name = "UpdateParticalVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]

    public async Task<IActionResult> UpdateParticalVilla(int id, JsonPatchDocument<VillaUpdateDTO>? patchDto)
    {
        if (patchDto == null || id == 0)
        {
            return BadRequest();
        }
        var villa = await _dbVilla.GetAsync(u => u.Id == id,tracked:false);
        
        //Convert types Villa to VillaDto (using AutoMapper(use MapperConfig))

        VillaUpdateDTO villaDto = _mapper.Map<VillaUpdateDTO>(villa);

        if (villa == null)
        {
            return BadRequest();
        }
        
        patchDto.ApplyTo(villaDto,ModelState);
        
        // Convert types VillaDto to Villa (using AutoMapper(use MapperConfig))
        Villa model = _mapper.Map<Villa>(villaDto);

        await _dbVilla.UpdateAsync(model);
        
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        return NoContent();
    }

}