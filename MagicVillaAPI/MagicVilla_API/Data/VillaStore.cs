using MagicVilla_API.Models.DTO;

namespace MagicVilla_API.Data;

public static class VillaStore
{
    public static List<VillaDTO> villaList = new List<VillaDTO>
    {
        new VillaDTO { Id = 1, Name = "Pool view", Occupants = 2, SquareMeters = 32},
        new VillaDTO { Id = 2, Name = "Beach view", Occupants = 6, SquareMeters = 54},
    };
}
