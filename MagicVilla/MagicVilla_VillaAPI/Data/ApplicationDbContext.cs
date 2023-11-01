using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data;
//Create Db; value db;
public class ApplicationDbContext:DbContext  
{
    public DbSet<Villa> Villas { get; set; }
    public DbSet<VillaNumber> VillaNumbers { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Add Data in Villa table
        modelBuilder.Entity<Villa>().HasData
        (
            new Villa()
            {
                Id = 1,
                Name = "Royal Villa",
                Details = "Royal Villa (1 smal room, 1 big room for 4 people(ocupance);",
                ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/blueVillaimages/villa3.jpg",
                Occupancy = 4,
                Sqft = 550,
                Rate = 200,
                Amenity = "",
                CreatedDate = DateTime.Now
            },
            new Villa()
            {
                Id = 2,
                Name = "Premium Pool Villa",
                Details = "Premium Pool Villa (2 big rooms for 4 people(ocupance) && 1 small  pool;",
                ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/blueVillaimages/villa1.jpg",
                Sqft = 550,
                Occupancy = 4,
                Rate = 300,
                Amenity= "",
                CreatedDate = DateTime.Now
            },
            new Villa()
            {
                Id = 3,
                Name = "Luxary Pool Villa",
                Details = "Luxary Pool Villa (2 big rooms for 4 people(ocupance) && 1 big pool;",
                ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/blueVillaimages/villa4.jpg",
                Sqft = 750,
                Occupancy = 4,
                Rate = 400,
                Amenity= "",
                CreatedDate = DateTime.Now
            },
            new Villa()
            {
                Id = 4,
                Name = "Diamond Villa",
                Details = "Diamond Pool Villa (2 very  big rooms for 4 people(ocupance);",
                ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/blueVillaimages/villa2.jpg",
                Sqft = 750,
                Occupancy = 4,
                Rate = 550,
                Amenity= "",
                CreatedDate = DateTime.Now
            },
            new Villa()
            {
                Id = 5,
                Name = "Diamond Pool Villa",
                Details = "Diamond Pool Villa (2 very big rooms for 4 people(ocupance) && 2 big pool;",
                ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/blueVillaimages/villa5.jpg",
                Sqft = 1100,
                Occupancy = 4,
                Rate = 600,
                Amenity= "",
                CreatedDate = DateTime.Now
            }
        );
    }
} 