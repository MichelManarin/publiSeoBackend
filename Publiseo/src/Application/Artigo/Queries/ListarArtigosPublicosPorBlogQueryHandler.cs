using Application.Artigo.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Artigo.Queries;

[ExcludeFromCodeCoverage]
public sealed class ListarArtigosPublicosPorBlogQueryHandler : IRequestHandler<ListarArtigosPublicosPorBlogQuery, IReadOnlyList<ArtigoPublicoResponse>?>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IArtigoRepository _artigoRepository;

    public ListarArtigosPublicosPorBlogQueryHandler(
        IBlogRepository blogRepository,
        IArtigoRepository artigoRepository)
    {
        _blogRepository = blogRepository;
        _artigoRepository = artigoRepository;
    }

    public async Task<IReadOnlyList<ArtigoPublicoResponse>?> Handle(ListarArtigosPublicosPorBlogQuery request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.ObterPorExternalIdAsync(request.BlogExternalId, cancellationToken);
        if (blog == null)
            return null;

        var artigos = await _artigoRepository.ListarPublicosPorBlogAsync(blog.Id, cancellationToken);
        var autorPadrao = string.IsNullOrWhiteSpace(blog.AutorPadraoNome)
            ? null
            : blog.AutorPadraoNome.Trim();

        return artigos.Select(a =>
        {
            var autor = autorPadrao ?? (a.UltimoUsuario != null
                ? $"{a.UltimoUsuario.Nome.Trim()} {a.UltimoUsuario.Sobrenome?.Trim()}".Trim()
                : string.Empty);

            return new ArtigoPublicoResponse(
                a.Titulo,
                a.MetaDescription,
                a.DataCriacao,
                a.Conteudo,
                autor,
                a.ImagemCapaUrl,
                a.ImagemCapaAttribution);
        }).ToList();
    }
}
