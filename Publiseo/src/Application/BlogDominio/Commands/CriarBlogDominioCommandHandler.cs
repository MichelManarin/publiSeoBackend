using Application.BlogDominio.Contracts;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.BlogDominio.Commands;

[ExcludeFromCodeCoverage]
public sealed class CriarBlogDominioCommandHandler : IRequestHandler<CriarBlogDominioCommand, BlogDominioResponse?>
{
    private readonly IBlogDominioRepository _blogDominioRepository;
    private readonly IBlogRepository _blogRepository;

    public CriarBlogDominioCommandHandler(
        IBlogDominioRepository blogDominioRepository,
        IBlogRepository blogRepository)
    {
        _blogDominioRepository = blogDominioRepository;
        _blogRepository = blogRepository;
    }

    public async Task<BlogDominioResponse?> Handle(CriarBlogDominioCommand request, CancellationToken cancellationToken)
    {
        var blogsDoUsuario = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        var blogAlvo = blogsDoUsuario.FirstOrDefault(b => b.Id == request.BlogId);
        if (blogAlvo == null)
            return null;

        var nomeDominio = request.NomeDominio.Trim().ToLowerInvariant();
        var existenteNoMesmoBlog = await _blogDominioRepository.ObterPorBlogENomeAsync(request.BlogId, nomeDominio, cancellationToken);
        if (existenteNoMesmoBlog != null)
            throw new BadRequestException($"O domínio {nomeDominio} já está vinculado a este blog.");

        var existenteNaPlataforma = await _blogDominioRepository.ObterPorNomeDominioAsync(nomeDominio, cancellationToken);
        if (existenteNaPlataforma != null)
        {
            var usuarioTemAcessoAoBlogDoDominio = blogsDoUsuario.Any(b => b.Id == existenteNaPlataforma.BlogId);
            if (usuarioTemAcessoAoBlogDoDominio)
                throw new BadRequestException($"O domínio já está vinculado ao blog com o nome {existenteNaPlataforma.Blog?.Nome ?? "N/A"}.");
            throw new BadRequestException("O domínio já se encontra configurado para outro usuário.");
        }

        var blogDominio = new Domain.Entities.BlogDominio
        {
            Id = Guid.NewGuid(),
            BlogId = request.BlogId,
            NomeDominio = nomeDominio
        };
        await _blogDominioRepository.InserirAsync(blogDominio, cancellationToken);

        return new BlogDominioResponse(blogDominio.Id, blogDominio.BlogId, blogDominio.NomeDominio);
    }
}
