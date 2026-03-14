using Application.SearchConsole.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.SearchConsole.Queries;

[ExcludeFromCodeCoverage]
public sealed class ListarMetricasSearchConsolePorBlogQueryHandler : IRequestHandler<ListarMetricasSearchConsolePorBlogQuery, IReadOnlyList<SearchConsoleMetricaResponse>?>
{
    private readonly IBlogRepository _blogRepository;
    private readonly ISearchConsoleMetricaRepository _metricaRepository;

    public ListarMetricasSearchConsolePorBlogQueryHandler(
        IBlogRepository blogRepository,
        ISearchConsoleMetricaRepository metricaRepository)
    {
        _blogRepository = blogRepository;
        _metricaRepository = metricaRepository;
    }

    public async Task<IReadOnlyList<SearchConsoleMetricaResponse>?> Handle(ListarMetricasSearchConsolePorBlogQuery request, CancellationToken cancellationToken)
    {
        var blogsDoUsuario = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        if (blogsDoUsuario.All(b => b.Id != request.BlogId))
            return null;

        var metricas = await _metricaRepository.ListarPorBlogAsync(request.BlogId, request.DataInicio, request.DataFim, cancellationToken);
        return metricas.Select(m => new SearchConsoleMetricaResponse(
            m.Id,
            m.BlogDominioId,
            m.BlogDominio.NomeDominio,
            m.Data,
            m.TipoBusca,
            m.Impressoes,
            m.Cliques,
            m.Ctr,
            m.PosicaoMedia,
            m.DataSincronizacao)).ToList();
    }
}
