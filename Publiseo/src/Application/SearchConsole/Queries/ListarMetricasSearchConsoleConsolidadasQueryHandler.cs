using Application.SearchConsole.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.SearchConsole.Queries;

[ExcludeFromCodeCoverage]
public sealed class ListarMetricasSearchConsoleConsolidadasQueryHandler : IRequestHandler<ListarMetricasSearchConsoleConsolidadasQuery, IReadOnlyList<SearchConsoleMetricaConsolidadaResponse>>
{
    private readonly IBlogRepository _blogRepository;
    private readonly ISearchConsoleMetricaRepository _metricaRepository;

    public ListarMetricasSearchConsoleConsolidadasQueryHandler(
        IBlogRepository blogRepository,
        ISearchConsoleMetricaRepository metricaRepository)
    {
        _blogRepository = blogRepository;
        _metricaRepository = metricaRepository;
    }

    public async Task<IReadOnlyList<SearchConsoleMetricaConsolidadaResponse>> Handle(ListarMetricasSearchConsoleConsolidadasQuery request, CancellationToken cancellationToken)
    {
        var blogs = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        var blogIds = blogs.Select(b => b.Id).ToList();
        if (blogIds.Count == 0)
            return Array.Empty<SearchConsoleMetricaConsolidadaResponse>();

        var metricas = await _metricaRepository.ListarPorBlogsAsync(blogIds, request.DataInicio, request.DataFim, cancellationToken);
        var agrupadas = metricas
            .GroupBy(m => new { m.Data, m.TipoBusca })
            .Select(g =>
            {
                var totalImpressoes = g.Sum(x => x.Impressoes);
                var totalCliques = g.Sum(x => x.Cliques);
                var ctr = totalImpressoes > 0 ? (double)totalCliques / totalImpressoes : 0d;
                var posicaoMedia = totalImpressoes > 0
                    ? g.Sum(x => x.PosicaoMedia * x.Impressoes) / totalImpressoes
                    : 0d;
                return new SearchConsoleMetricaConsolidadaResponse(
                    g.Key.Data,
                    g.Key.TipoBusca,
                    totalImpressoes,
                    totalCliques,
                    ctr,
                    posicaoMedia);
            })
            .OrderBy(x => x.Data)
            .ThenBy(x => x.TipoBusca)
            .ToList();

        return agrupadas;
    }
}
