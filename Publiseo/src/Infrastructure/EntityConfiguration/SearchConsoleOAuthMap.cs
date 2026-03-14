using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class SearchConsoleOAuthMap : IEntityTypeConfiguration<SearchConsoleOAuth>
{
    public void Configure(EntityTypeBuilder<SearchConsoleOAuth> builder)
    {
        builder.ToTable("search_console_oauth");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UsuarioId).IsRequired();
        builder.Property(x => x.RefreshToken).IsRequired();
        builder.Property(x => x.EmailGoogle).HasMaxLength(256);
        builder.Property(x => x.DataVinculo).IsRequired();

        builder.HasOne(x => x.Usuario)
            .WithOne(x => x.SearchConsoleOAuth)
            .HasForeignKey<SearchConsoleOAuth>(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.UsuarioId).IsUnique();
    }
}
