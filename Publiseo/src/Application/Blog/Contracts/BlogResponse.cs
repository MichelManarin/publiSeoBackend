namespace Application.Blog.Contracts;

public record BlogResponse(
    Guid Id,
    Guid ExternalId,
    Guid UsuarioId,
    string Nome,
    string? UrlSlug,
    string Nicho,
    string? Descricao,
    IReadOnlyList<string> PalavrasChave,
    string? AutorPadraoNome,
    DateTime DataCriacao);
