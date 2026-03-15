using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class ConversorRepository : IConversorRepository
{
    private readonly PubliseoDbContext _context;

    public ConversorRepository(PubliseoDbContext context) => _context = context;

    public async Task<Conversor?> ObterPorBlogIdAsync(Guid blogId, CancellationToken cancellationToken = default)
        => await _context.Conversores.FirstOrDefaultAsync(c => c.BlogId == blogId, cancellationToken);

    public async Task<Conversor?> ObterPorBlogIdComPerguntasAsync(Guid blogId, CancellationToken cancellationToken = default)
        => await _context.Conversores
            .Include(c => c.Perguntas.OrderBy(p => p.Ordem))
            .FirstOrDefaultAsync(c => c.BlogId == blogId, cancellationToken);

    public async Task<Conversor?> ObterPorBlogExternalIdAsync(Guid blogExternalId, CancellationToken cancellationToken = default)
        => await _context.Conversores
            .Include(c => c.Perguntas.OrderBy(p => p.Ordem))
            .FirstOrDefaultAsync(c => c.Blog.ExternalId == blogExternalId, cancellationToken);

    public async Task<Conversor> InserirAsync(Conversor conversor, CancellationToken cancellationToken = default)
    {
        _context.Conversores.Add(conversor);
        await _context.SaveChangesAsync(cancellationToken);
        return conversor;
    }

    public async Task<Conversor> AtualizarAsync(Conversor conversor, CancellationToken cancellationToken = default)
    {
        conversor.DataAtualizacao = DateTime.UtcNow;
        _context.Conversores.Update(conversor);
        await _context.SaveChangesAsync(cancellationToken);
        return conversor;
    }
}
