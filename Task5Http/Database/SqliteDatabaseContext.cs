using DatabaseEntitiesLib;
using DatabaseEntitiesLib.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Task5Http.Database
{
    public class SqliteDatabaseContext : AbstractHackatonContext
    {
        public SqliteDatabaseContext(DbContextOptions options) : base(options)
        {
            Database.EnsureDeleted();
            Database.Migrate();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new JuniorConfiguration());
            modelBuilder.ApplyConfiguration(new TeamleadConfiguration());
            modelBuilder.ApplyConfiguration(new HackatonConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new JuniorPreferenceConfiguration());
            modelBuilder.ApplyConfiguration(new TeamleadPreferenceConfiguration());
        }
    }
}
