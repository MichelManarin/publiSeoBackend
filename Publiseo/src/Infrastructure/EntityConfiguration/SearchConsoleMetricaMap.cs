using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class SearchConsoleMetricaMap : IEntityTypeConfiguration<SearchConsoleMetrica>
{
    public void Configure(EntityTypeBuilder<SearchConsoleMetrica> builder)
    {
        builder.ToTable("search_console_metricas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BlogDominioId).IsRequired();
        builder.Property(x => x.Data).IsRequired();
        builder.Property(x => x.TipoBusca).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Impressoes).IsRequired();
        builder.Property(x => x.Cliques).IsRequired();
        builder.Property(x => x.Ctr).IsRequired();
        builder.Property(x => x.PosicaoMedia).IsRequired();
        builder.Property(x => x.DataSincronizacao).IsRequired();

        builder.HasOne(x => x.BlogDominio)
            .WithMany(x => x.MetricasSearchConsole)
            .HasForeignKey(x => x.BlogDominioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.BlogDominioId, x.Data, x.TipoBusca }).IsUnique();
    }
}
