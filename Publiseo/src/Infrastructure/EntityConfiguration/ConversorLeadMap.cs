using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class ConversorLeadMap : IEntityTypeConfiguration<ConversorLead>
{
    public void Configure(EntityTypeBuilder<ConversorLead> builder)
    {
        builder.ToTable("conversor_leads");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ConversorId).IsRequired();
        builder.Property(x => x.NomeCompleto).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Telefone).HasMaxLength(30).IsRequired();
        builder.Property(x => x.RespostasJson).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.ArtigoId);
        builder.Property(x => x.Ip).HasMaxLength(45);
        builder.Property(x => x.DataCriacao).IsRequired();

        builder.HasOne(x => x.Conversor)
            .WithMany(x => x.Leads)
            .HasForeignKey(x => x.ConversorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
