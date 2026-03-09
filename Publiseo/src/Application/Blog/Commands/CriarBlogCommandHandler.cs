using Application.Blog.Contracts;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Blog.Commands;

[ExcludeFromCodeCoverage]
public sealed class CriarBlogCommandHandler : IRequestHandler<CriarBlogCommand, BlogResponse?>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IBlogMembroRepository _blogMembroRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public CriarBlogCommandHandler(
        IBlogRepository blogRepository,
        IBlogMembroRepository blogMembroRepository,
        IUsuarioRepository usuarioRepository)
    {
        _blogRepository = blogRepository;
        _blogMembroRepository = blogMembroRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<BlogResponse?> Handle(CriarBlogCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(request.UsuarioId, cancellationToken);
        if (usuario == null)
            return null;

        var palavrasChave = (request.PalavrasChave ?? Array.Empty<string>())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Take(5)
            .Select(p => p!.Trim())
            .ToList();

        var blog = new Domain.Entities.Blog
        {
            Id = Guid.NewGuid(),
            UsuarioId = request.UsuarioId,
            Nome = request.Nome.Trim(),
            Nicho = request.Nicho.Trim(),
            PalavrasChave = palavrasChave,
            UrlSlug = string.IsNullOrWhiteSpace(request.UrlSlug) ? null : request.UrlSlug.Trim().ToLowerInvariant(),
            DataCriacao = DateTime.UtcNow
        };
        await _blogRepository.InserirAsync(blog, cancellationToken);

        var membro = new Domain.Entities.BlogMembro
        {
            Id = Guid.NewGuid(),
            BlogId = blog.Id,
            UsuarioId = request.UsuarioId,
            Papel = PapelBlog.Owner,
            DataVinculo = DateTime.UtcNow
        };
        await _blogMembroRepository.InserirAsync(membro, cancellationToken);

        return new BlogResponse(
            blog.Id,
            blog.UsuarioId,
            blog.Nome,
            blog.UrlSlug,
            blog.Nicho,
            blog.PalavrasChave,
            blog.DataCriacao);
    }
}
