using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task4Database.Data.Entity;

namespace Task4Database.Data.Configuration
{
    public class TeamleadPreferenceConfiguration : IEntityTypeConfiguration<TeamleadPreference>
    {
        public void Configure(EntityTypeBuilder<TeamleadPreference> builder)
        {
            builder.ToTable("teamlead_preference");
            builder.HasKey(p => new { p.HackatonId, p.JuniorId, p.TeamleadId });
            builder.Property(p => p.HackatonId).HasColumnName("hackaton_id");
            builder.Property(p => p.TeamleadId).HasColumnName("teamlead_id");
            builder.Property(p => p.JuniorId).HasColumnName("junior_id");
            builder.Property(p => p.Priority).HasColumnName("priority").IsRequired();

            builder.HasOne(p => p.Hackaton).WithMany().HasForeignKey(p => p.HackatonId);
            builder.HasOne(p => p.Teamlead).WithMany().HasForeignKey(p => p.TeamleadId);
            builder.HasOne(p => p.PreferredJunior).WithMany().HasForeignKey(p => p.JuniorId);
        }
    }
}
