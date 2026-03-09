using Application.Blog.Contracts;
using MediatR;

namespace Application.Blog.Commands;

public record CriarBlogCommand(
    Guid UsuarioId,
    string Nome,
    string Nicho,
    IReadOnlyList<string> PalavrasChave,
    string? UrlSlug = null) : IRequest<BlogResponse?>;
