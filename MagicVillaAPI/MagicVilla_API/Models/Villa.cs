using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace MagicVilla_API.Models;

public class Villa
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //el id se autoincrementa de uno en uno y se asigne automaticamente
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Detail { get; set; }

    [Required]
    public double Fee { get; set; }

    public int Occupants { get; set; }

    public int SquareMeters { get; set; }

    public string? ImageUrl { get; set; }

    public string? Comfort { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ActualDate { get; set; }
}
