using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<User> Users { get; set; }
    }
}
