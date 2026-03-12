using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Artigo.Commands;

[ExcludeFromCodeCoverage]
public sealed class ExcluirArtigoCommandHandler : IRequestHandler<ExcluirArtigoCommand, bool>
{
    private readonly IArtigoRepository _artigoRepository;
    private readonly IBlogRepository _blogRepository;

    public ExcluirArtigoCommandHandler(
        IArtigoRepository artigoRepository,
        IBlogRepository blogRepository)
    {
        _artigoRepository = artigoRepository;
        _blogRepository = blogRepository;
    }

    public async Task<bool> Handle(ExcluirArtigoCommand request, CancellationToken cancellationToken)
    {
        var artigo = await _artigoRepository.ObterPorIdAsync(request.Id, cancellationToken);
        if (artigo == null)
            return false;

        var blogsDoUsuario = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        if (blogsDoUsuario.All(b => b.Id != artigo.BlogId))
            return false;

        await _artigoRepository.ExcluirAsync(request.Id, cancellationToken);
        return true;
    }
}
