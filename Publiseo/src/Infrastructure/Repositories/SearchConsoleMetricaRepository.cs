using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class SearchConsoleMetricaRepository : ISearchConsoleMetricaRepository
{
    private readonly PubliseoDbContext _context;

    public SearchConsoleMetricaRepository(PubliseoDbContext context) => _context = context;

    public async Task<bool> ExisteAlgumaMetricaParaBlogsAsync(IEnumerable<Guid> blogIds, CancellationToken cancellationToken = default)
    {
        var ids = blogIds.ToList();
        if (ids.Count == 0) return false;
        return await _context.SearchConsoleMetricas
            .AnyAsync(x => ids.Contains(x.BlogDominio.BlogId), cancellationToken);
    }

    public async Task<SearchConsoleMetrica?> ObterPorDominioDataETipoAsync(Guid blogDominioId, DateOnly data, string tipoBusca, CancellationToken cancellationToken = default)
        => await _context.SearchConsoleMetricas
            .FirstOrDefaultAsync(x => x.BlogDominioId == blogDominioId && x.Data == data && x.TipoBusca == tipoBusca, cancellationToken);

    public async Task<IReadOnlyList<SearchConsoleMetrica>> ListarPorBlogDominioAsync(Guid blogDominioId, DateOnly dataInicio, DateOnly dataFim, CancellationToken cancellationToken = default)
        => await _context.SearchConsoleMetricas
            .Where(x => x.BlogDominioId == blogDominioId && x.Data >= dataInicio && x.Data <= dataFim)
            .OrderBy(x => x.Data)
            .ThenBy(x => x.TipoBusca)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<SearchConsoleMetrica>> ListarPorBlogAsync(Guid blogId, DateOnly dataInicio, DateOnly dataFim, CancellationToken cancellationToken = default)
        => await _context.SearchConsoleMetricas
            .AsNoTracking()
            .Where(x => x.BlogDominio.BlogId == blogId && x.Data >= dataInicio && x.Data <= dataFim)
            .Include(x => x.BlogDominio)
            .OrderBy(x => x.Data)
            .ThenBy(x => x.BlogDominio.NomeDominio)
            .ThenBy(x => x.TipoBusca)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<SearchConsoleMetrica>> ListarPorBlogsAsync(IEnumerable<Guid> blogIds, DateOnly dataInicio, DateOnly dataFim, CancellationToken cancellationToken = default)
    {
        var ids = blogIds.ToList();
        if (ids.Count == 0) return Array.Empty<SearchConsoleMetrica>();
        return await _context.SearchConsoleMetricas
            .AsNoTracking()
            .Where(x => ids.Contains(x.BlogDominio.BlogId) && x.Data >= dataInicio && x.Data <= dataFim)
            .Include(x => x.BlogDominio)
            .OrderBy(x => x.Data)
            .ThenBy(x => x.TipoBusca)
            .ToListAsync(cancellationToken);
    }

    public async Task InserirOuAtualizarAsync(SearchConsoleMetrica metrica, CancellationToken cancellationToken = default)
    {
        var existente = await ObterPorDominioDataETipoAsync(metrica.BlogDominioId, metrica.Data, metrica.TipoBusca, cancellationToken);
        if (existente != null)
        {
            existente.Impressoes = metrica.Impressoes;
            existente.Cliques = metrica.Cliques;
            existente.Ctr = metrica.Ctr;
            existente.PosicaoMedia = metrica.PosicaoMedia;
            existente.DataSincronizacao = metrica.DataSincronizacao;
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            _context.SearchConsoleMetricas.Add(metrica);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
