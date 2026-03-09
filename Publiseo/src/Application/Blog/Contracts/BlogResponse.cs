namespace Application.Blog.Contracts;

public record BlogResponse(
    Guid Id,
    Guid UsuarioId,
    string Nome,
    string? UrlSlug,
    string Nicho,
    IReadOnlyList<string> PalavrasChave,
    DateTime DataCriacao);
