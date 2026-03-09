using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class BlogMembroMap : IEntityTypeConfiguration<BlogMembro>
{
    public void Configure(EntityTypeBuilder<BlogMembro> builder)
    {
        builder.ToTable("blog_membros");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BlogId).IsRequired();
        builder.Property(x => x.UsuarioId).IsRequired();
        builder.Property(x => x.Papel).IsRequired();
        builder.Property(x => x.DataVinculo).IsRequired();

        builder.HasIndex(x => new { x.BlogId, x.UsuarioId }).IsUnique();

        builder.HasOne(x => x.Usuario)
            .WithMany(x => x.BlogMembros)
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}