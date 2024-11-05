using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task4Database.Data.Entity;

namespace Task4Database.Data.Configuration
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("team");
            builder.HasKey(t => new { t.HackatonId, t.JuniorId, t.TeamleadId });
            builder.Property(t => t.HackatonId).HasColumnName("hackaton_id");
            builder.Property(t => t.JuniorId).HasColumnName("junior_id");
            builder.Property(t => t.TeamleadId).HasColumnName("teamlead_id");

            builder.HasOne(t => t.Hackaton).WithMany(h => h.Teams).HasForeignKey(t => t.HackatonId);
            builder.HasOne(t => t.Junior).WithMany().HasForeignKey(t => t.JuniorId);
            builder.HasOne(t => t.Teamlead).WithMany().HasForeignKey(t => t.TeamleadId);
        }
    }
}
