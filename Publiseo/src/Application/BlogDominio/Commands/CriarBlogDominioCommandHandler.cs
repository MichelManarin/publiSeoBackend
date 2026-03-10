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
        var blog = await _blogRepository.ObterPorIdAsync(request.BlogId, cancellationToken);
        if (blog == null)
            return null;

        var nomeDominio = request.NomeDominio.Trim().ToLowerInvariant();
        var existente = await _blogDominioRepository.ObterPorBlogENomeAsync(request.BlogId, nomeDominio, cancellationToken);
        if (existente != null)
            throw new BadRequestException($"O domínio {nomeDominio} já está vinculado a este blog.");

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
