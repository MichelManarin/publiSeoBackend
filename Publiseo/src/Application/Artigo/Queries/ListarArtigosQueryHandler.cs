using Application.Artigo.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Artigo.Queries;

[ExcludeFromCodeCoverage]
public sealed class ListarArtigosQueryHandler : IRequestHandler<ListarArtigosQuery, IReadOnlyList<ArtigoResponse>>
{
    private readonly IArtigoRepository _artigoRepository;

    public ListarArtigosQueryHandler(IArtigoRepository artigoRepository) => _artigoRepository = artigoRepository;

    public async Task<IReadOnlyList<ArtigoResponse>> Handle(ListarArtigosQuery request, CancellationToken cancellationToken)
    {
        var artigos = await _artigoRepository.ListarPorBlogAsync(request.BlogId, cancellationToken);
        return artigos.Select(a => new ArtigoResponse(
            a.Id,
            a.BlogId,
            a.Titulo,
            a.MetaDescription,
            a.Conteudo,
            a.TipoRascunho,
            a.DataCriacao,
            a.DataAtualizacao,
            a.UltimoUsuarioId)).ToList();
    }
}
