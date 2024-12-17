using DatabaseEntitiesLib.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseEntitiesLib.Configuration
{
    public class JuniorConfiguration : IEntityTypeConfiguration<Junior>
    {
        public void Configure(EntityTypeBuilder<Junior> builder)
        {
            builder.ToTable("junior");
            builder.HasKey(j => j.Id);
            builder.Property(j => j.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(j => j.Name).HasColumnName("name").IsRequired();
        }
    }
}
