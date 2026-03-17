using Application.BlogIntegracao.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.BlogIntegracao.Queries;

[ExcludeFromCodeCoverage]
public sealed class ObterIntegracoesPublicasPorBlogQueryHandler : IRequestHandler<ObterIntegracoesPublicasPorBlogQuery, IReadOnlyList<IntegracaoPublicaItemDto>>
{
    private readonly IBlogIntegracaoRepository _integracaoRepository;

    public ObterIntegracoesPublicasPorBlogQueryHandler(IBlogIntegracaoRepository integracaoRepository)
        => _integracaoRepository = integracaoRepository;

    public async Task<IReadOnlyList<IntegracaoPublicaItemDto>> Handle(ObterIntegracoesPublicasPorBlogQuery request, CancellationToken cancellationToken)
    {
        var lista = await _integracaoRepository.ListarPorBlogExternalIdAsync(request.BlogExternalId, cancellationToken);
        return lista.Select(i => new IntegracaoPublicaItemDto(i.Tipo.ToString(), i.Valor)).ToList();
    }
}
