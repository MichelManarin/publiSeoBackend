using Domain.Enums;

namespace Application.Artigo.Contracts;

public record ArtigoResponse(
    Guid Id,
    Guid BlogId,
    string Titulo,
    string? MetaDescription,
    string Conteudo,
    TipoRascunho TipoRascunho,
    StatusGeracaoArtigo? StatusGeracao,
    int TentativasGeracao,
    DateTime DataCriacao,
    DateTime DataAtualizacao,
    Guid UltimoUsuarioId,
    bool Excluido,
    string? ImagemCapaUrl = null,
    string? ImagemCapaUnsplashId = null,
    string? ImagemCapaAttribution = null);
