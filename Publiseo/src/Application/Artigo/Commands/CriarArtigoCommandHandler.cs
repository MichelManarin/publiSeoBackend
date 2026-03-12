using Application.Artigo.Contracts;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Artigo.Commands;

[ExcludeFromCodeCoverage]
public sealed class CriarArtigoCommandHandler : IRequestHandler<CriarArtigoCommand, ArtigoResponse?>
{
    private readonly IArtigoRepository _artigoRepository;
    private readonly IBlogRepository _blogRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public CriarArtigoCommandHandler(
        IArtigoRepository artigoRepository,
        IBlogRepository blogRepository,
        IUsuarioRepository usuarioRepository)
    {
        _artigoRepository = artigoRepository;
        _blogRepository = blogRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ArtigoResponse?> Handle(CriarArtigoCommand request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.ObterPorIdAsync(request.BlogId, cancellationToken);
        if (blog == null)
            return null;

        var usuario = await _usuarioRepository.ObterPorIdAsync(request.UsuarioId, cancellationToken);
        if (usuario == null)
            return null;

        var agora = DateTime.UtcNow;
        var ehPorIa = request.TipoRascunho == TipoRascunho.IA;
        var artigo = new Domain.Entities.Artigo
        {
            Id = Guid.NewGuid(),
            BlogId = request.BlogId,
            Titulo = request.Titulo.Trim(),
            MetaDescription = string.IsNullOrWhiteSpace(request.MetaDescription) ? null : request.MetaDescription.Trim(),
            Conteudo = ehPorIa ? string.Empty : (request.Conteudo ?? string.Empty),
            TipoRascunho = request.TipoRascunho,
            StatusGeracao = ehPorIa ? StatusGeracaoArtigo.Pendente : null,
            TentativasGeracao = 0,
            DataCriacao = agora,
            DataAtualizacao = agora,
            UltimoUsuarioId = request.UsuarioId
        };

        await _artigoRepository.InserirAsync(artigo, cancellationToken);

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
            artigo.UltimoUsuarioId,
            false);
    }
}
