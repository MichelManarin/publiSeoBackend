using Application.BlogDominio.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.BlogDominio.Queries;

[ExcludeFromCodeCoverage]
public sealed class ListarDominiosPorBlogQueryHandler : IRequestHandler<ListarDominiosPorBlogQuery, IReadOnlyList<BlogDominioResponse>?>
{
    private readonly IBlogDominioRepository _blogDominioRepository;
    private readonly IBlogRepository _blogRepository;

    public ListarDominiosPorBlogQueryHandler(
        IBlogDominioRepository blogDominioRepository,
        IBlogRepository blogRepository)
    {
        _blogDominioRepository = blogDominioRepository;
        _blogRepository = blogRepository;
    }

    public async Task<IReadOnlyList<BlogDominioResponse>?> Handle(ListarDominiosPorBlogQuery request, CancellationToken cancellationToken)
    {
        var blogsDoUsuario = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        if (blogsDoUsuario.All(b => b.Id != request.BlogId))
            return null;

        var dominios = await _blogDominioRepository.ListarPorBlogAsync(request.BlogId, cancellationToken);
        return dominios.Select(d => new BlogDominioResponse(d.Id, d.BlogId, d.NomeDominio)).ToList();
    }
}
