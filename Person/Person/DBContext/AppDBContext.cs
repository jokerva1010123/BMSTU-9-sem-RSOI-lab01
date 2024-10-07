using Microsoft.EntityFrameworkCore;
using Model;

namespace DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<PersonDB> People { get; set; }
    }
}
