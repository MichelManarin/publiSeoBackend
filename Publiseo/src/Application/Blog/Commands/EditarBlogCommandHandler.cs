using Application.Blog.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Blog.Commands;

[ExcludeFromCodeCoverage]
public sealed class EditarBlogCommandHandler : IRequestHandler<EditarBlogCommand, BlogResponse?>
{
    private readonly IBlogRepository _blogRepository;

    public EditarBlogCommandHandler(IBlogRepository blogRepository) => _blogRepository = blogRepository;

    public async Task<BlogResponse?> Handle(EditarBlogCommand request, CancellationToken cancellationToken)
    {
        var blogsDoUsuario = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        var blog = blogsDoUsuario.FirstOrDefault(b => b.Id == request.BlogId);
        if (blog == null)
            return null;

        var palavrasChave = (request.PalavrasChave ?? Array.Empty<string>())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Take(5)
            .Select(p => p!.Trim())
            .ToList();

        blog.Nome = request.Nome.Trim();
        blog.Nicho = request.Nicho.Trim();
        blog.PalavrasChave = palavrasChave;
        blog.UrlSlug = string.IsNullOrWhiteSpace(request.UrlSlug) ? null : request.UrlSlug.Trim().ToLowerInvariant();
        blog.Descricao = string.IsNullOrWhiteSpace(request.Descricao) ? null : request.Descricao.Trim();
        blog.AutorPadraoNome = string.IsNullOrWhiteSpace(request.AutorPadraoNome) ? null : request.AutorPadraoNome.Trim();

        await _blogRepository.AtualizarAsync(blog, cancellationToken);

        return new BlogResponse(
            blog.Id,
            blog.ExternalId,
            blog.UsuarioId,
            blog.Nome,
            blog.UrlSlug,
            blog.Nicho,
            blog.Descricao,
            blog.PalavrasChave,
            blog.AutorPadraoNome,
            blog.DataCriacao);
    }
}
