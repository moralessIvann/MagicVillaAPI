using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_API.Models;

public class VillaNum
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)] //allowed to user assing villa number
    public int VillaNo { get; set; }
    
    [Required]
    public int VillaId { get; set; }

    [ForeignKey("VillaId")] // relation with Villa model
    public Villa Villa { get; set; }

    public string Details { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ActualDate { get; set; }
}
