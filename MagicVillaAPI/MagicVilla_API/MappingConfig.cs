using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTO;

namespace MagicVilla_API;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Villa,VillaDTO>();
        CreateMap<VillaDTO, Villa>();

        CreateMap<Villa, VillaCreateDTO>().ReverseMap();
        CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

        CreateMap<VillaNum, VillaNumDTO>().ReverseMap();
        CreateMap<VillaNum, VillaNumCreateDTO>().ReverseMap();
        CreateMap<VillaNum, VillaNumUpdateDTO>().ReverseMap();
    }
}
