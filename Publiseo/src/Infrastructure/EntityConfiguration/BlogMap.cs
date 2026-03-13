using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
namespace Infrastructure.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class BlogMap : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.ToTable("blogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ExternalId).IsRequired();
        builder.HasIndex(x => x.ExternalId).IsUnique();

        builder.Property(x => x.UsuarioId).IsRequired();
        builder.Property(x => x.Nome).HasMaxLength(300).IsRequired();
        builder.Property(x => x.UrlSlug).HasMaxLength(300);
        builder.Property(x => x.Nicho).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Descricao).HasMaxLength(2000);
        builder.Property(x => x.AutorPadraoNome).HasMaxLength(200);
        builder.Property(x => x.ObjetivoFinal).HasMaxLength(100);
        builder.Property(x => x.PossuiProdutoVinculado);
        builder.Property(x => x.DescricaoProdutoVinculado).HasMaxLength(2000);
        builder.Property(x => x.DataCriacao).IsRequired();

        builder.Property(x => x.PalavrasChave)
            .HasConversion(new PalavrasChaveJsonConverter())
            .HasColumnType("jsonb");

        builder.HasIndex(x => new { x.UsuarioId, x.UrlSlug }).IsUnique().HasFilter("\"UrlSlug\" IS NOT NULL");

        builder.HasMany(x => x.Membros)
            .WithOne(x => x.Blog)
            .HasForeignKey(x => x.BlogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
