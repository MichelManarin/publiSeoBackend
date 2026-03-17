using Application.Blog.Contracts;
using Application.BlogIntegracao.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Application.Blog.Queries;

[ExcludeFromCodeCoverage]
public sealed class ObterBlogPorDominioQueryHandler : IRequestHandler<ObterBlogPorDominioQuery, BlogPorDominioResponse?>
{
    private readonly IBlogDominioRepository _blogDominioRepository;
    private readonly IBlogIntegracaoRepository _integracaoRepository;
    private readonly ILogger<ObterBlogPorDominioQueryHandler> _logger;

    public ObterBlogPorDominioQueryHandler(
        IBlogDominioRepository blogDominioRepository,
        IBlogIntegracaoRepository integracaoRepository,
        ILogger<ObterBlogPorDominioQueryHandler> logger)
    {
        _blogDominioRepository = blogDominioRepository;
        _integracaoRepository = integracaoRepository;
        _logger = logger;
    }

    public async Task<BlogPorDominioResponse?> Handle(ObterBlogPorDominioQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Procurando por blog com vinculo com o dominio: {Dominio}", request.Dominio);
        var blogDominio = await _blogDominioRepository.ObterPorNomeDominioAsync(request.Dominio, cancellationToken);
        if (blogDominio?.Blog == null)
            return null;
        var blog = blogDominio.Blog;
        var integracoes = await _integracaoRepository.ListarPorBlogExternalIdAsync(blog.ExternalId, cancellationToken);
        var integracoesDto = integracoes.Select(i => new IntegracaoPublicaItemDto(i.Tipo.ToString(), i.Valor)).ToList();
        return new BlogPorDominioResponse(blog.ExternalId, blog.Nome, blog.Nicho, blog.Descricao, integracoesDto);
    }
}
