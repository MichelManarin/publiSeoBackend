using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class DominioRepository : IDominioRepository
{
    private readonly PubliseoDbContext _context;

    public DominioRepository(PubliseoDbContext context) => _context = context;

    public async Task<Dominio?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Dominios.FindAsync([id], cancellationToken);

    public async Task<Dominio?> ObterPorUsuarioENomeAsync(Guid usuarioId, string nomeDominio, CancellationToken cancellationToken = default)
        => await _context.Dominios
            .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.NomeDominio == nomeDominio, cancellationToken);

    public async Task<IReadOnlyList<Dominio>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default)
        => await _context.Dominios
            .Where(x => x.UsuarioId == usuarioId)
            .OrderBy(x => x.NomeDominio)
            .ToListAsync(cancellationToken);

    public async Task<Dominio> InserirAsync(Dominio dominio, CancellationToken cancellationToken = default)
    {
        _context.Dominios.Add(dominio);
        await _context.SaveChangesAsync(cancellationToken);
        return dominio;
    }
}
