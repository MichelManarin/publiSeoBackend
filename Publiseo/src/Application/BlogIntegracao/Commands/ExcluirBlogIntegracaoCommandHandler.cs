using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.BlogIntegracao.Commands;

[ExcludeFromCodeCoverage]
public sealed class ExcluirBlogIntegracaoCommandHandler : IRequestHandler<ExcluirBlogIntegracaoCommand, bool>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IBlogIntegracaoRepository _integracaoRepository;

    public ExcluirBlogIntegracaoCommandHandler(
        IBlogRepository blogRepository,
        IBlogIntegracaoRepository integracaoRepository)
    {
        _blogRepository = blogRepository;
        _integracaoRepository = integracaoRepository;
    }

    public async Task<bool> Handle(ExcluirBlogIntegracaoCommand request, CancellationToken cancellationToken)
    {
        var blogsDoUsuario = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        if (blogsDoUsuario.All(b => b.Id != request.BlogId))
            return false;

        var integracao = await _integracaoRepository.ObterPorIdAsync(request.IntegracaoId, cancellationToken);
        if (integracao == null || integracao.BlogId != request.BlogId)
            return false;

        await _integracaoRepository.ExcluirAsync(integracao, cancellationToken);
        return true;
    }
}
