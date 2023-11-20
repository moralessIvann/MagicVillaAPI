using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.DTO;

public class VillaNumUpdateDTO
{
    [Required]
    public int VillaNo { get; set; }

    [Required]
    public int VillaId { get; set; }

    public string Details { get; set; }
}
