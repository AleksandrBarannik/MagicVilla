﻿using AutoMapper;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models;
namespace MagicVilla_Web;

//Config Сопоставления Villa и VillaDTo между собой(в обе стороны)
public class MappingConfig:Profile
{
    public MappingConfig()
    {
        CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
        CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();
        CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
        CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();
    }
}