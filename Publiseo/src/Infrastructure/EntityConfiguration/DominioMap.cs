using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class DominioMap : IEntityTypeConfiguration<Dominio>
{
    public void Configure(EntityTypeBuilder<Dominio> builder)
    {
        builder.ToTable("dominios");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.NomeDominio).HasMaxLength(253).IsRequired();
        builder.Property(x => x.DataCompra).IsRequired();
        builder.Property(x => x.DataExpiracao).IsRequired();
        builder.Property(x => x.OrdemIdExterno);
        builder.Property(x => x.Total).HasPrecision(18, 4);
        builder.Property(x => x.Moeda).HasMaxLength(10);
        builder.Property(x => x.PeriodoAnos).IsRequired();
        builder.Property(x => x.Privacy).IsRequired();
        builder.Property(x => x.RenewAuto).IsRequired();

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.UsuarioId, x.NomeDominio }).IsUnique();
    }
}
