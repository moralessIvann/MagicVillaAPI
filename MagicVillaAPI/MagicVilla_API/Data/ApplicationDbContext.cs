using MagicVilla_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Villa> Villas { get; set; } //esto es una tabla en la bd

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Villa>().HasData(
            new Villa()
            {
                Id = 1,
                Name = "Hood Villa",
                Detail = "Regular",
                ImageUrl = "",
                Occupants = 23,
                SquareMeters = 76,
                Fee = 5,
                Comfort = "",
                CreatedDate = DateTime.Now,
                ActualDate = DateTime.Now,
            },
            new Villa()
            {
                Id = 2,
                Name = "Cite Villa",
                Detail = "Full",
                ImageUrl = "",
                Occupants = 6,
                SquareMeters = 34,
                Fee = 12,
                Comfort = "",
                CreatedDate = DateTime.Now,
                ActualDate = DateTime.Now,
            });
    }
}
