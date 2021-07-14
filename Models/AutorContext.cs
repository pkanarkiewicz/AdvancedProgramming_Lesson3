using Microsoft.EntityFrameworkCore;

namespace AdvancedProgramming_Lesson3.Models
{
    public class AutorContext : DbContext
    {

        public AutorContext(DbContextOptions<AutorContext> options)
            : base(options)
        {
        }

        public DbSet<AutorItem> AutorItems { get; set; }
    }
}
