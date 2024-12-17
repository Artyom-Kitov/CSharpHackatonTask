using DatabaseEntitiesLib.Configuration;
using DatabaseEntitiesLib.Entity;
using Microsoft.EntityFrameworkCore;

namespace DatabaseEntitiesLib
{
    public abstract class AbstractHackatonContext : DbContext
    {
        public DbSet<Junior> Juniors { get; set; } = null!;
        public DbSet<Teamlead> Teamleads { get; set; } = null!;
        public DbSet<Hackaton> Hackatons { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<JuniorPreference> JuniorPreferences { get; set; } = null!;
        public DbSet<TeamleadPreference> TeamleadPreferences { get; set; } = null!;

        public AbstractHackatonContext(DbContextOptions options) : base(options) { }

        public AbstractHackatonContext() : base() { }

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
