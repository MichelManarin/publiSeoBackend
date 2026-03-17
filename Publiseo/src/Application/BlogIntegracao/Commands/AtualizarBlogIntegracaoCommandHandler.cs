using Application.BlogIntegracao.Contracts;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.BlogIntegracao.Commands;

[ExcludeFromCodeCoverage]
public sealed class AtualizarBlogIntegracaoCommandHandler : IRequestHandler<AtualizarBlogIntegracaoCommand, BlogIntegracaoItemDto?>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IBlogIntegracaoRepository _integracaoRepository;

    public AtualizarBlogIntegracaoCommandHandler(
        IBlogRepository blogRepository,
        IBlogIntegracaoRepository integracaoRepository)
    {
        _blogRepository = blogRepository;
        _integracaoRepository = integracaoRepository;
    }

    public async Task<BlogIntegracaoItemDto?> Handle(AtualizarBlogIntegracaoCommand request, CancellationToken cancellationToken)
    {
        var blogsDoUsuario = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        if (blogsDoUsuario.All(b => b.Id != request.BlogId))
            return null;

        var integracao = await _integracaoRepository.ObterPorIdAsync(request.IntegracaoId, cancellationToken);
        if (integracao == null || integracao.BlogId != request.BlogId)
            return null;

        if (request.Tipo != null)
            integracao.Tipo = Enum.Parse<BlogIntegracaoTipo>(request.Tipo.Trim(), ignoreCase: true);
        if (request.Valor != null)
            integracao.Valor = request.Valor.Trim();
        if (request.Ordem.HasValue)
            integracao.Ordem = request.Ordem.Value;

        await _integracaoRepository.AtualizarAsync(integracao, cancellationToken);
        return new BlogIntegracaoItemDto(integracao.Id, integracao.Tipo.ToString(), integracao.Valor, integracao.Ordem, integracao.DataCriacao, integracao.DataAtualizacao);
    }
}
