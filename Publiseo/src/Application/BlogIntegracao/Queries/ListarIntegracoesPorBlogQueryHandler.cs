using Application.BlogIntegracao.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.BlogIntegracao.Queries;

[ExcludeFromCodeCoverage]
public sealed class ListarIntegracoesPorBlogQueryHandler : IRequestHandler<ListarIntegracoesPorBlogQuery, IReadOnlyList<BlogIntegracaoItemDto>>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IBlogIntegracaoRepository _integracaoRepository;

    public ListarIntegracoesPorBlogQueryHandler(
        IBlogRepository blogRepository,
        IBlogIntegracaoRepository integracaoRepository)
    {
        _blogRepository = blogRepository;
        _integracaoRepository = integracaoRepository;
    }

    public async Task<IReadOnlyList<BlogIntegracaoItemDto>> Handle(ListarIntegracoesPorBlogQuery request, CancellationToken cancellationToken)
    {
        var blogsDoUsuario = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        if (blogsDoUsuario.All(b => b.Id != request.BlogId))
            return Array.Empty<BlogIntegracaoItemDto>();

        var lista = await _integracaoRepository.ListarPorBlogIdAsync(request.BlogId, cancellationToken);
        return lista.Select(i => new BlogIntegracaoItemDto(i.Id, i.Tipo.ToString(), i.Valor, i.Ordem, i.DataCriacao, i.DataAtualizacao)).ToList();
    }
}
