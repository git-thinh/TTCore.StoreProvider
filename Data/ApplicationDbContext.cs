using TTCore.StoreProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace TTCore.StoreProvider.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Request> Requests { get; set; }
    }
}
