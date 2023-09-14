using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.DTO;

public class VillaDTO
{
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string? Name { get; set; }

    public string? Detail { get; set; }

    [Required]
    public double Fee { get; set; }

    public int Occupants { get; set; }

    public int SquareMeters { get; set; }

    public string? ImageUrl { get; set; }

    public string? Comfort { get; set; }
}
