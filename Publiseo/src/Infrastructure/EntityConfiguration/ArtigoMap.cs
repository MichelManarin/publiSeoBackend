using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class ArtigoMap : IEntityTypeConfiguration<Artigo>
{
    public void Configure(EntityTypeBuilder<Artigo> builder)
    {
        builder.ToTable("artigos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BlogId).IsRequired();
        builder.Property(x => x.Titulo).HasMaxLength(500).IsRequired();
        builder.Property(x => x.MetaDescription).HasMaxLength(500);
        builder.Property(x => x.Conteudo).HasColumnType("text").IsRequired();
        builder.Property(x => x.TipoRascunho).IsRequired();
        builder.Property(x => x.DataCriacao).IsRequired();
        builder.Property(x => x.DataAtualizacao).IsRequired();
        builder.Property(x => x.UltimoUsuarioId).IsRequired();

        builder.HasOne(x => x.Blog)
            .WithMany()
            .HasForeignKey(x => x.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.UltimoUsuario)
            .WithMany()
            .HasForeignKey(x => x.UltimoUsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.BlogId);
    }
}
