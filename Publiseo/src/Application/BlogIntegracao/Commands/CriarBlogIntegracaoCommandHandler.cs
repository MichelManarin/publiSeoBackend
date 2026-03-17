using Application.BlogIntegracao.Contracts;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.BlogIntegracao.Commands;

[ExcludeFromCodeCoverage]
public sealed class CriarBlogIntegracaoCommandHandler : IRequestHandler<CriarBlogIntegracaoCommand, BlogIntegracaoItemDto?>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IBlogIntegracaoRepository _integracaoRepository;

    public CriarBlogIntegracaoCommandHandler(
        IBlogRepository blogRepository,
        IBlogIntegracaoRepository integracaoRepository)
    {
        _blogRepository = blogRepository;
        _integracaoRepository = integracaoRepository;
    }

    public async Task<BlogIntegracaoItemDto?> Handle(CriarBlogIntegracaoCommand request, CancellationToken cancellationToken)
    {
        var blogsDoUsuario = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        if (blogsDoUsuario.All(b => b.Id != request.BlogId))
            return null;

        var tipo = Enum.Parse<BlogIntegracaoTipo>(request.Tipo.Trim(), ignoreCase: true);
        var valor = request.Valor?.Trim() ?? string.Empty;
        var now = DateTime.UtcNow;

        var integracao = new Domain.Entities.BlogIntegracao
        {
            Id = Guid.NewGuid(),
            BlogId = request.BlogId,
            Tipo = tipo,
            Valor = valor,
            Ordem = request.Ordem,
            DataCriacao = now,
            DataAtualizacao = now
        };
        await _integracaoRepository.InserirAsync(integracao, cancellationToken);

        return new BlogIntegracaoItemDto(integracao.Id, integracao.Tipo.ToString(), integracao.Valor, integracao.Ordem, integracao.DataCriacao, integracao.DataAtualizacao);
    }
}
