using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("images");

            builder.HasKey(i => i.Id);

            builder.Property(a => a.Id)
                   .HasColumnName("id")
                   .HasColumnType("bigint");

            builder.Property(i => i.Description)
                   .HasColumnName("description")
                   .IsRequired(false)
                   .HasMaxLength(400);

        }
    }
}
