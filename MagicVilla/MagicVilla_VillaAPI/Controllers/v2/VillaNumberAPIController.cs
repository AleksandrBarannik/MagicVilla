using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

// Run  HTTP METHODS Get for Table VillaNumbers Version 2.0

namespace MagicVilla_VillaAPI.Controllers.v2;

[Route("api/v{version:apiVersion}/VillaNumberAPI")]
[ApiController]
[ApiVersion("2.0")]
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
    public IEnumerable<string> Get()
    {
        return new[] { "value1", "value2" };
    }
}