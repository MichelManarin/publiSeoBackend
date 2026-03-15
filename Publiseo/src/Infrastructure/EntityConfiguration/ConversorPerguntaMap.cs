using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class ConversorPerguntaMap : IEntityTypeConfiguration<ConversorPergunta>
{
    public void Configure(EntityTypeBuilder<ConversorPergunta> builder)
    {
        builder.ToTable("conversor_perguntas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ConversorId).IsRequired();
        builder.Property(x => x.Ordem).IsRequired();
        builder.Property(x => x.TextoPergunta).HasMaxLength(500).IsRequired();
        builder.Property(x => x.TipoCampo).IsRequired();

        builder.HasOne(x => x.Conversor)
            .WithMany(x => x.Perguntas)
            .HasForeignKey(x => x.ConversorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
