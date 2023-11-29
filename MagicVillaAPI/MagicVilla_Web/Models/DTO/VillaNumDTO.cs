using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models;

public class VillaNumDTO
{
    [Required]
    public int VillaNo { get; set; }

    [Required]
    public int VillaId { get; set; }

    public string Details { get; set; }
}
