using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("articles");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                   .HasColumnName("id")
                   .HasColumnType("bigint");

            builder.Property(a => a.Title)
                   .HasColumnName("title")
                   .IsRequired(false)
                   .HasMaxLength(200);

            builder.HasIndex(a => a.Title);

            builder.HasMany(a => a.Author)
                   .WithMany()
                   .UsingEntity<Dictionary<string, object>>(
                       "article_author",
                       j => j.HasOne<Author>()
                             .WithMany()
                             .HasForeignKey("author_id")
                             .OnDelete(DeleteBehavior.SetNull),
                       j => j.HasOne<Article>()
                             .WithMany()
                             .HasForeignKey("article_id")
                             .OnDelete(DeleteBehavior.SetNull),
                       j =>
                       {
                           j.ToTable("article_author");
                           j.HasKey("article_id", "author_id");
                       });

            builder.HasOne(a => a.Site)
                   .WithMany()
                   .HasForeignKey("site_id")
                   .OnDelete(DeleteBehavior.SetNull);
        }

    }
}
