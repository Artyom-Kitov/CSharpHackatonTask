using Microsoft.EntityFrameworkCore;
using Task4Database.Data.Configuration;
using Task4Database.Data.Entity;

namespace Task5Http.Database
{
    public class SqliteDatabaseContext : DbContext
    {
        public DbSet<Junior> Juniors { get; set; } = null!;
        public DbSet<Teamlead> Teamleads { get; set; } = null!;
        public DbSet<Hackaton> Hackatons { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<JuniorPreference> JuniorPreferences { get; set; } = null!;
        public DbSet<TeamleadPreference> TeamleadPreferences { get; set; } = null!;

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
