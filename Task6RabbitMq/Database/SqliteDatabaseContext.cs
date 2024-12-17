using DatabaseEntitiesLib;
using DatabaseEntitiesLib.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Task6RabbitMq.Database
{
    public class SqliteDatabaseContext : AbstractHackatonContext
    {
        public SqliteDatabaseContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
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
