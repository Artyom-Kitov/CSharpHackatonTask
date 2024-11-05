using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task4Database.Data.Entity;

namespace Task4Database.Data.Configuration
{
    public class JuniorPreferenceConfiguration : IEntityTypeConfiguration<JuniorPreference>
    {
        public void Configure(EntityTypeBuilder<JuniorPreference> builder)
        {
            builder.ToTable("junior_preference");
            builder.HasKey(p => new { p.HackatonId, p.JuniorId, p.TeamleadId });
            builder.Property(p => p.HackatonId).HasColumnName("hackaton_id");
            builder.Property(p => p.JuniorId).HasColumnName("junior_id");
            builder.Property(p => p.TeamleadId).HasColumnName("teamlead_id");
            builder.Property(p => p.Priority).HasColumnName("priority").IsRequired();

            builder.HasOne(p => p.Hackaton).WithMany().HasForeignKey(p => p.HackatonId);
            builder.HasOne(p => p.Junior).WithMany().HasForeignKey(p => p.JuniorId);
            builder.HasOne(p => p.PreferredTeamlead).WithMany().HasForeignKey(p => p.TeamleadId);
        }
    }
}
