using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Models.Dto.Identity;

namespace MagicVilla_VillaAPI;

//Config Сопоставления Villa и VillaDTo между собой(в обе стороны)
public class MappingConfig:Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDTO>().ReverseMap();
        CreateMap<Villa, VillaCreateDTO>().ReverseMap();
        CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        
        CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();;
        CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
        
    }
}