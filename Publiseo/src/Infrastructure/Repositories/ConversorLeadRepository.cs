using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class ConversorLeadRepository : IConversorLeadRepository
{
    private readonly PubliseoDbContext _context;

    public ConversorLeadRepository(PubliseoDbContext context) => _context = context;

    public async Task<ConversorLead> InserirAsync(ConversorLead lead, CancellationToken cancellationToken = default)
    {
        _context.ConversorLeads.Add(lead);
        await _context.SaveChangesAsync(cancellationToken);
        return lead;
    }
}
