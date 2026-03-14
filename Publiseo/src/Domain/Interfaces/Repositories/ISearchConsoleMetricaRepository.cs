using Domain.Entities;

namespace Domain.Interfaces.Repositories;

/// <summary>
/// Repositório de métricas do Search Console (agregadas por domínio e dia).
/// </summary>
public interface ISearchConsoleMetricaRepository
{
    Task<bool> ExisteAlgumaMetricaParaBlogsAsync(IEnumerable<Guid> blogIds, CancellationToken cancellationToken = default);
    Task<SearchConsoleMetrica?> ObterPorDominioDataETipoAsync(Guid blogDominioId, DateOnly data, string tipoBusca, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SearchConsoleMetrica>> ListarPorBlogDominioAsync(Guid blogDominioId, DateOnly dataInicio, DateOnly dataFim, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SearchConsoleMetrica>> ListarPorBlogAsync(Guid blogId, DateOnly dataInicio, DateOnly dataFim, CancellationToken cancellationToken = default);
    Task InserirOuAtualizarAsync(SearchConsoleMetrica metrica, CancellationToken cancellationToken = default);
}
