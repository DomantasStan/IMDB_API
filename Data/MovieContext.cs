using IMDB_API.Models;
using Microsoft.EntityFrameworkCore;

namespace IMDB_API.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
            
        }

        public DbSet<Movie> Movies { get; set; }
    }
}
