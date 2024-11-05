using Microsoft.EntityFrameworkCore;
using Task4Database.Data.Configuration;
using Task4Database.Data.Entity;

namespace Task4Database.Database
{
    public class ApplicationDbContext : DbContext
    {
        private readonly DatabaseConfig _config;

        public DbSet<Junior> Juniors { get; set; } = null!;
        public DbSet<Teamlead> Teamleads { get; set; } = null!;
        public DbSet<Hackaton> Hackatons { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<JuniorPreference> JuniorPreferences { get; set; } = null!;
        public DbSet<TeamleadPreference> TeamleadPreferences { get; set; } = null!;

        public ApplicationDbContext(DatabaseConfig config)
        {
            _config = config;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_config.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new JuniorConfiguration());
            modelBuilder.ApplyConfiguration(new TeamleadConfiguration());
            modelBuilder.ApplyConfiguration(new HackatonConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new JuniorPreferenceConfiguration());
            modelBuilder.ApplyConfiguration(new TeamleadPreferenceConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
