using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class BlogDominioMap : IEntityTypeConfiguration<BlogDominio>
{
    public void Configure(EntityTypeBuilder<BlogDominio> builder)
    {
        builder.ToTable("blog_dominios");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BlogId).IsRequired();
        builder.Property(x => x.NomeDominio).HasMaxLength(253).IsRequired();

        builder.HasOne(x => x.Blog)
            .WithMany(x => x.Dominios)
            .HasForeignKey(x => x.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.BlogId, x.NomeDominio }).IsUnique();
    }
}
