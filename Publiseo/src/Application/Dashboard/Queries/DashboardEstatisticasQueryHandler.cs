using Application.Dashboard.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Dashboard.Queries;

[ExcludeFromCodeCoverage]
public sealed class DashboardEstatisticasQueryHandler : IRequestHandler<DashboardEstatisticasQuery, DashboardEstatisticasResponse>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IArtigoRepository _artigoRepository;

    public DashboardEstatisticasQueryHandler(
        IBlogRepository blogRepository,
        IArtigoRepository artigoRepository)
    {
        _blogRepository = blogRepository;
        _artigoRepository = artigoRepository;
    }

    public async Task<DashboardEstatisticasResponse> Handle(DashboardEstatisticasQuery request, CancellationToken cancellationToken)
    {
        var blogs = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        var blogIds = blogs.Select(b => b.Id).ToList();
        var totalArtigos = await _artigoRepository.ContarPorBlogIdsAsync(blogIds, cancellationToken);
        return new DashboardEstatisticasResponse(blogs.Count, totalArtigos);
    }
}
