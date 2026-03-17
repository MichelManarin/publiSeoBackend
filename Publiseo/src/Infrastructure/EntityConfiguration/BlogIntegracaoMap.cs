using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class BlogIntegracaoMap : IEntityTypeConfiguration<BlogIntegracao>
{
    public void Configure(EntityTypeBuilder<BlogIntegracao> builder)
    {
        builder.ToTable("blog_integracoes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BlogId).IsRequired();
        builder.Property(x => x.Tipo).HasConversion<string>().HasMaxLength(100).IsRequired();
        builder.Property(x => x.Valor).HasColumnType("text").IsRequired();
        builder.Property(x => x.Ordem).IsRequired();
        builder.Property(x => x.DataCriacao).IsRequired();
        builder.Property(x => x.DataAtualizacao).IsRequired();

        builder.HasOne(x => x.Blog)
            .WithMany()
            .HasForeignKey(x => x.BlogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
