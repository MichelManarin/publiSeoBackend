using Application.BlogIntegracao.Contracts;
using MediatR;

namespace Application.BlogIntegracao.Queries;

/// <summary>
/// Lista integrações do blog por ExternalId para o front injetar no &lt;head&gt; (endpoint público).
/// </summary>
public record ObterIntegracoesPublicasPorBlogQuery(Guid BlogExternalId) : IRequest<IReadOnlyList<IntegracaoPublicaItemDto>>;
