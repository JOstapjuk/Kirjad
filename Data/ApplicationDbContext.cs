using Kirjad.Models;
using Microsoft.EntityFrameworkCore;

namespace Kirjad.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Kiri> Kirjad { get; set; }
        public DbSet<Veebiuudis> Veebiuudised { get; set; }
    }
}
