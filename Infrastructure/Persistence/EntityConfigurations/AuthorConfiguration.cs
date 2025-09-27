using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("authors");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                   .HasColumnName("id")
                   .HasColumnType("bigint");

            builder.Property(a => a.Name)
                   .HasColumnName("name")
                   .IsRequired(false)
                   .HasMaxLength(200);

            builder.HasIndex(a => a.Name)
                   .IsUnique();

            builder.HasOne(a => a.Image)
                   .WithOne()
                   .HasForeignKey<Author>("image_id")
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
