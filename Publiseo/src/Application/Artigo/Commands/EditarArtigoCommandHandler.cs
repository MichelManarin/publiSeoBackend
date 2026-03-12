using Application.Artigo.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Artigo.Commands;

[ExcludeFromCodeCoverage]
public sealed class EditarArtigoCommandHandler : IRequestHandler<EditarArtigoCommand, ArtigoResponse?>
{
    private readonly IArtigoRepository _artigoRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public EditarArtigoCommandHandler(
        IArtigoRepository artigoRepository,
        IUsuarioRepository usuarioRepository)
    {
        _artigoRepository = artigoRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ArtigoResponse?> Handle(EditarArtigoCommand request, CancellationToken cancellationToken)
    {
        var artigo = await _artigoRepository.ObterPorIdAsync(request.Id, cancellationToken);
        if (artigo == null)
            return null;

        var usuario = await _usuarioRepository.ObterPorIdAsync(request.UsuarioId, cancellationToken);
        if (usuario == null)
            return null;

        artigo.Titulo = request.Titulo.Trim();
        artigo.MetaDescription = string.IsNullOrWhiteSpace(request.MetaDescription) ? null : request.MetaDescription.Trim();
        artigo.Conteudo = request.Conteudo;
        artigo.TipoRascunho = request.TipoRascunho;
        artigo.DataAtualizacao = DateTime.UtcNow;
        artigo.UltimoUsuarioId = request.UsuarioId;

        await _artigoRepository.AtualizarAsync(artigo, cancellationToken);

        return new ArtigoResponse(
            artigo.Id,
            artigo.BlogId,
            artigo.Titulo,
            artigo.MetaDescription,
            artigo.Conteudo,
            artigo.TipoRascunho,
            artigo.StatusGeracao,
            artigo.TentativasGeracao,
            artigo.DataCriacao,
            artigo.DataAtualizacao,
            artigo.UltimoUsuarioId);
    }
}
