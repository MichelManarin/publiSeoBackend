using Application.Blog.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Blog.Queries;

[ExcludeFromCodeCoverage]
public sealed class ListarBlogsPorUsuarioQueryHandler : IRequestHandler<ListarBlogsPorUsuarioQuery, IReadOnlyList<BlogResponse>>
{
    private readonly IBlogRepository _blogRepository;

    public ListarBlogsPorUsuarioQueryHandler(IBlogRepository blogRepository) => _blogRepository = blogRepository;

    public async Task<IReadOnlyList<BlogResponse>> Handle(ListarBlogsPorUsuarioQuery request, CancellationToken cancellationToken)
    {
        var blogs = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        return blogs.Select(b => new BlogResponse(b.Id, b.ExternalId, b.UsuarioId, b.Nome, b.UrlSlug, b.Nicho, b.PalavrasChave, b.DataCriacao)).ToList();
    }
}
