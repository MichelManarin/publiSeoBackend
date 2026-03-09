using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class UsuarioMap : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Sobrenome).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Telefone).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Login).HasMaxLength(256).IsRequired();
        builder.Property(x => x.SenhaHash).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Endereco).HasMaxLength(300);
        builder.Property(x => x.Cidade).HasMaxLength(100);
        builder.Property(x => x.Estado).HasMaxLength(2);
        builder.Property(x => x.CodigoPostal).HasMaxLength(20);
        builder.Property(x => x.Pais).HasMaxLength(100).IsRequired();
        builder.Property(x => x.UltimoLogin);
        builder.Property(x => x.UltimoTokenGerado).HasColumnType("text");
        builder.Property(x => x.DataCriacao).IsRequired();
        builder.Property(x => x.UltimoIp).HasMaxLength(45);

        builder.HasIndex(x => x.Login).IsUnique();
        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasMany(x => x.Blogs)
            .WithOne(x => x.Usuario)
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
