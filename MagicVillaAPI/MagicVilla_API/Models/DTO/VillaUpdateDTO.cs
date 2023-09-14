using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.DTO;

public class VillaUpdateDTO
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string? Name { get; set; }

    public string? Detail { get; set; }

    [Required]
    public double Fee { get; set; }

    [Required]
    public int Occupants { get; set; }

    [Required]
    public int SquareMeters { get; set; }

    [Required]
    public string? ImageUrl { get; set; }

    public string? Comfort { get; set; }
}
