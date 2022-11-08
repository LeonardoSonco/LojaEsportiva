using Registerservice.Models;

using Microsoft.EntityFrameworkCore;

namespace Registerservice.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Register> Registers { get; set; }
    }
}
