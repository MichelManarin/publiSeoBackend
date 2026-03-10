using Domain.Enums;

namespace Application.Artigo.Contracts;

public record ArtigoResponse(
    Guid Id,
    Guid BlogId,
    string Titulo,
    string? MetaDescription,
    string Conteudo,
    TipoRascunho TipoRascunho,
    DateTime DataCriacao,
    DateTime DataAtualizacao,
    Guid UltimoUsuarioId);
