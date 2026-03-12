using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.BlogDominio.Commands;

[ExcludeFromCodeCoverage]
public sealed class RemoverBlogDominioCommandHandler : IRequestHandler<RemoverBlogDominioCommand, bool>
{
    private readonly IBlogDominioRepository _blogDominioRepository;
    private readonly IBlogRepository _blogRepository;

    public RemoverBlogDominioCommandHandler(
        IBlogDominioRepository blogDominioRepository,
        IBlogRepository blogRepository)
    {
        _blogDominioRepository = blogDominioRepository;
        _blogRepository = blogRepository;
    }

    public async Task<bool> Handle(RemoverBlogDominioCommand request, CancellationToken cancellationToken)
    {
        var blogDominio = await _blogDominioRepository.ObterPorIdAsync(request.BlogDominioId, cancellationToken);
        if (blogDominio == null)
            return false;

        var blogsDoUsuario = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        if (blogsDoUsuario.All(b => b.Id != blogDominio.BlogId))
            return false;

        await _blogDominioRepository.ExcluirAsync(request.BlogDominioId, cancellationToken);
        return true;
    }
}
