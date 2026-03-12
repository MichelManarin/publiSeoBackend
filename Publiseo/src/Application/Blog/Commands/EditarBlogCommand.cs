using Application.Blog.Contracts;
using MediatR;

namespace Application.Blog.Commands;

public record EditarBlogCommand(
    Guid UsuarioId,
    Guid BlogId,
    string Nome,
    string Nicho,
    IReadOnlyList<string> PalavrasChave,
    string? UrlSlug = null,
    string? Descricao = null,
    string? AutorPadraoNome = null) : IRequest<BlogResponse?>;
