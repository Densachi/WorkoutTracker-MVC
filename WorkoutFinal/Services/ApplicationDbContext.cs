using Microsoft.EntityFrameworkCore;
using WorkoutFinal.Models;

namespace WorkoutFinal.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        
        
        }

        public DbSet<Exercise> Exercises { get; set; }

    }
}
