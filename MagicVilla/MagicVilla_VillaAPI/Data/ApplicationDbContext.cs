using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data;
//Create Db
public class ApplicationDbContext:DbContext  
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
    }
    public DbSet<Villa> Villas { get; set; }
} 