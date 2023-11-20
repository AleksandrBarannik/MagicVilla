using System.Net;
using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

// Run  HTTP METHODS (GET;POST;PUT;DELETE;PATCH;) for Table Villas

namespace MagicVilla_VillaAPI.Controllers.v1;

//Set route
[Route("api/v{version:apiVersion}/VillaAPI")]
[ApiController]
[ApiVersion("1.0")]
public class VillaApiController: ControllerBase
{
    protected ApiResponse _response;
    private readonly IVillaRepository _dbVilla;
    private readonly IMapper _mapper;
    
    public VillaApiController(IVillaRepository dbVilla,IMapper mapper)
    {
        _dbVilla = dbVilla;
        _mapper = mapper;
        this._response = new();
    }

    [HttpGet]
    [ResponseCache(CacheProfileName = "Default30")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async  Task<ActionResult<ApiResponse>> GetVillas()
    {
        try
        {
            IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
            _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpGet("{id:int}",Name = "GetVilla")]
    [ResponseCache(Duration = 30)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound )]
    public async Task<ActionResult<ApiResponse>> GetVilla(int id)
    {//С помощью ActionResult определяем тип возвращаемого значения
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villa = await _dbVilla.GetAsync(u => u.Id == id);

            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpPost] 
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError )]
    public async Task<ActionResult<ApiResponse>>CreateVilla([FromBody]VillaCreateDTO createDto)
    {
        try
        {
            if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("ErrorMessages", "Villa already Exsist");
                return BadRequest(ModelState);
            }

            if (createDto == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            //Convert types VillaDto to Villa (using AutoMapper(use MapperConfig))
            Villa villa = _mapper.Map<Villa>(createDto);
            await _dbVilla.CreateAsync(villa);

            _response.Result = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>>DeleteVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villa = await _dbVilla.GetAsync(u => u.Id == id);

            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _dbVilla.RemoveAsync(villa);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        
        
        return _response;
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<ApiResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDto)
    {
        try
        {
            if (updateDto == null || id != updateDto.Id)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            //Convert types VillaDto to Villa (using AutoMapper(use MapperConfig))
            Villa villa = _mapper.Map<Villa>(updateDto);
            await _dbVilla.UpdateAsync(villa);

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpPatch("{id:int}", Name = "UpdateParticalVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<ApiResponse>> UpdateParticalVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDto)
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
        _response.StatusCode = HttpStatusCode.NoContent;
        _response.IsSuccess = true;
        return Ok(_response);
    }
}