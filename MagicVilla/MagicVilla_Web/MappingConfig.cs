using AutoMapper;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models;
namespace MagicVilla_Web;

//Config Сопоставления Villa и VillaDTo между собой(в обе стороны)
public class MappingConfig:Profile
{
    public MappingConfig()
    {
        CreateMap<VillaDTO, Villa>().ReverseMap();
        CreateMap<Villa, VillaCreateDTO>().ReverseMap();
        CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        
        CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();;
        CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
    }
}