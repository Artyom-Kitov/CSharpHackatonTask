using DatabaseEntitiesLib.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseEntitiesLib.Configuration
{
    public class HackatonConfiguration : IEntityTypeConfiguration<Hackaton>
    {
        public void Configure(EntityTypeBuilder<Hackaton> builder)
        {
            builder.ToTable("hackaton");
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(h => h.Harmony).HasColumnName("harmony").IsRequired();

            builder.HasMany(h => h.Teams).WithOne(t => t.Hackaton).HasForeignKey(h => h.HackatonId);
        }
    }
}
