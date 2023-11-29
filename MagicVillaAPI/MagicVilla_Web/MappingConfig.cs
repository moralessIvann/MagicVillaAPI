using AutoMapper;
using MagicVilla_Web.Models;

namespace MagicVilla_Web;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
        CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();
        
        CreateMap<VillaNumDTO, VillaNumCreateDTO>().ReverseMap();
        CreateMap<VillaNumDTO, VillaNumUpdateDTO>().ReverseMap();
    }
}
