using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class ConversorMap : IEntityTypeConfiguration<Conversor>
{
    public void Configure(EntityTypeBuilder<Conversor> builder)
    {
        builder.ToTable("conversores");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BlogId).IsRequired();
        builder.Property(x => x.Ativo).IsRequired();
        builder.Property(x => x.TextoBotaoInicial).HasMaxLength(200);
        builder.Property(x => x.TipoFinalizacao).IsRequired();
        builder.Property(x => x.MensagemFinalizacao).HasMaxLength(2000);
        builder.Property(x => x.WhatsAppNumero).HasMaxLength(20);
        builder.Property(x => x.WhatsAppTextoPreDefinido).HasMaxLength(1000);
        builder.Property(x => x.DataCriacao).IsRequired();
        builder.Property(x => x.DataAtualizacao).IsRequired();

        builder.HasOne(x => x.Blog)
            .WithMany()
            .HasForeignKey(x => x.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.BlogId).IsUnique();
    }
}
