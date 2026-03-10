using Application.Blog.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Blog.Queries;

[ExcludeFromCodeCoverage]
public sealed class ObterBlogPorDominioQueryHandler : IRequestHandler<ObterBlogPorDominioQuery, BlogPorDominioResponse?>
{
    private readonly IBlogDominioRepository _blogDominioRepository;

    public ObterBlogPorDominioQueryHandler(IBlogDominioRepository blogDominioRepository)
        => _blogDominioRepository = blogDominioRepository;

    public async Task<BlogPorDominioResponse?> Handle(ObterBlogPorDominioQuery request, CancellationToken cancellationToken)
    {
        var blogDominio = await _blogDominioRepository.ObterPorNomeDominioAsync(request.Dominio, cancellationToken);
        if (blogDominio?.Blog == null)
            return null;
        return new BlogPorDominioResponse(blogDominio.Blog.ExternalId);
    }
}
