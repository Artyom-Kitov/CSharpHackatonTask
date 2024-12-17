using DatabaseEntitiesLib;
using Microsoft.EntityFrameworkCore;

namespace Task4Database.Database
{
    public class ApplicationDbContext : AbstractHackatonContext
    {
        private readonly DatabaseConfig _config;

        public ApplicationDbContext(DatabaseConfig config)
        {
            _config = config;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_config.ConnectionString);
        }
    }
}
