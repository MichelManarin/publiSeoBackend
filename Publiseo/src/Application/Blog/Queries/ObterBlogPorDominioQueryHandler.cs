using Application.Blog.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Application.Blog.Queries;

[ExcludeFromCodeCoverage]
public sealed class ObterBlogPorDominioQueryHandler : IRequestHandler<ObterBlogPorDominioQuery, BlogPorDominioResponse?>
{
    private readonly IBlogDominioRepository _blogDominioRepository;
    private readonly ILogger<ObterBlogPorDominioQueryHandler> _logger;

    public ObterBlogPorDominioQueryHandler(
        IBlogDominioRepository blogDominioRepository,
        ILogger<ObterBlogPorDominioQueryHandler> logger)
    {
        _blogDominioRepository = blogDominioRepository;
        _logger = logger;
    }

    public async Task<BlogPorDominioResponse?> Handle(ObterBlogPorDominioQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Procurando por blog com vinculo com o dominio: {Dominio}", request.Dominio);
        var blogDominio = await _blogDominioRepository.ObterPorNomeDominioAsync(request.Dominio, cancellationToken);
        if (blogDominio?.Blog == null)
            return null;
        var blog = blogDominio.Blog;
        return new BlogPorDominioResponse(blog.ExternalId, blog.Nome, blog.Nicho, blog.Descricao);
    }
}
