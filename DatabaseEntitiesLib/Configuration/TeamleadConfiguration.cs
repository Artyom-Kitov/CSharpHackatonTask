using DatabaseEntitiesLib.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseEntitiesLib.Configuration
{
    public class TeamleadConfiguration : IEntityTypeConfiguration<Teamlead>
    {
        public void Configure(EntityTypeBuilder<Teamlead> builder)
        {
            builder.ToTable("teamlead");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(t => t.Name).HasColumnName("name").IsRequired();
        }
    }
}
