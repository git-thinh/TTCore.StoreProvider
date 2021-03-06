using TTCore.StoreProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace TTCore.StoreProvider.Data
{
    public class DbMemoryContext : DbContext
    {
        public DbMemoryContext(DbContextOptions<DbMemoryContext> options) : base(options) { }

        public DbSet<Request> Requests { get; set; }
        public DbSet<MessageDto> Messages { get; set; }
    }
}
