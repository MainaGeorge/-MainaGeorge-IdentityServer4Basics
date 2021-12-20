using Microsoft.EntityFrameworkCore;
using Movies.API.Models;

namespace Movies.API.Data
{
    public class MoviesContext : DbContext
    {
        public MoviesContext(DbContextOptions<MoviesContext> opts) : base(opts)
        {
        }

        public DbSet<Movie> Movies { get; set; } 
    }
}
