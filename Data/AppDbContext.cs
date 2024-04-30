using Plats.Models;
using Microsoft.EntityFrameworkCore;
 
namespace Plats.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Platinum> Plats { get; set; }
 
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("DataSource=platinum.sqlite;Cache=Shared");
    }
}