using Microsoft.EntityFrameworkCore;

namespace AdvancedProgramming_Lesson3.Models
{
    public class FilmyContext : DbContext
    {

        public FilmyContext(DbContextOptions<FilmyContext> options)
            : base(options)
        {
        }

        public DbSet<FilmyItem> FilmyItems { get; set; }
    }
}