using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class SearchConsoleOAuthRepository : ISearchConsoleOAuthRepository
{
    private readonly PubliseoDbContext _context;

    public SearchConsoleOAuthRepository(PubliseoDbContext context) => _context = context;

    public async Task<SearchConsoleOAuth?> ObterPorUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
        => await _context.Set<SearchConsoleOAuth>().FirstOrDefaultAsync(x => x.UsuarioId == usuarioId, cancellationToken);

    public async Task<IReadOnlyList<SearchConsoleOAuth>> ListarTodosComTokenAsync(CancellationToken cancellationToken = default)
        => await _context.Set<SearchConsoleOAuth>()
            .Where(x => !string.IsNullOrEmpty(x.RefreshToken))
            .ToListAsync(cancellationToken);

    public async Task<SearchConsoleOAuth> InserirOuAtualizarAsync(SearchConsoleOAuth oauth, CancellationToken cancellationToken = default)
    {
        var existente = await ObterPorUsuarioIdAsync(oauth.UsuarioId, cancellationToken);
        if (existente != null)
        {
            existente.RefreshToken = oauth.RefreshToken;
            existente.EmailGoogle = oauth.EmailGoogle;
            existente.DataVinculo = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            return existente;
        }
        _context.Set<SearchConsoleOAuth>().Add(oauth);
        await _context.SaveChangesAsync(cancellationToken);
        return oauth;
    }

    public async Task RemoverPorUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        var existente = await ObterPorUsuarioIdAsync(usuarioId, cancellationToken);
        if (existente != null)
        {
            _context.Set<SearchConsoleOAuth>().Remove(existente);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
