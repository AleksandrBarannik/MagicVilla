using System.Net;
using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

// Run  HTTP METHODS (GET;POST;PUT;DELETE;PATCH;) for Table VillaNumbers

namespace MagicVilla_VillaAPI.Controllers;

[Route("api/VillaNumberAPI")]
[ApiController]
public class VillaNumberAPIController: ControllerBase
{
    protected ApiResponse _response;
    private readonly IMapper _mapper;
    private readonly IVillaNumberRepository _dbVillaNumber;
    private readonly IVillaRepository _dbVilla;
    
    public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber,IVillaRepository dbVilla,IMapper mapper)
    {
        _mapper = mapper;
        _dbVilla = dbVilla;
        _dbVillaNumber = dbVillaNumber;
        this._response = new();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async   Task<ActionResult<ApiResponse>> GetVillaNumbers()
    {
        try
        {
            IEnumerable<VillaNumber> villaList = await _dbVillaNumber.GetAllAsync(includeProperties:"Villa");
            _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaList);
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

    [HttpGet("{id:int}",Name = "GetVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> GetVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            
            var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);

            if (villaNumber == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDto)
    {
        try
        {
            if (await _dbVillaNumber.GetAsync(u => u.VillaNo == createDto.VillaNo) != null)
            {
                ModelState.AddModelError("ErrorMessages", "Villa already Exsist");
                return BadRequest(ModelState);
            }

            if (await _dbVilla.GetAsync(u => u.Id == createDto.VillaID) == null)
            {
                ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid");
                return BadRequest(ModelState);
            }
            if (createDto == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var villaNumber = _mapper.Map<VillaNumber>(createDto);
            await _dbVillaNumber.CreateAsync(villaNumber);
            
            _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetVilla", new { id = villaNumber.VillaNo }, villaNumber);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpDelete("{id:int}",Name = "DeleteVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> DeleteVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);

            if (villaNumber == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            
            await _dbVillaNumber.RemoveAsync(villaNumber);
            _response.StatusCode = HttpStatusCode.OK;
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
    [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<ApiResponse>> UpdateVilla(int id, [FromBody] VillaNumberUpdateDTO updateDto)
    {
        try
        {
            if (updateDto == null || id != updateDto.VillaNo)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            
            if (await _dbVilla.GetAsync(u => u.Id == updateDto.VillaID) == null)
            {
                ModelState.AddModelError("CustomError", "Villa ID is Invalid");
                return BadRequest(ModelState);
            }

            VillaNumber villaNumber = _mapper.Map<VillaNumber>(updateDto);
            await _dbVillaNumber.UpdateAsync(villaNumber);

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
}