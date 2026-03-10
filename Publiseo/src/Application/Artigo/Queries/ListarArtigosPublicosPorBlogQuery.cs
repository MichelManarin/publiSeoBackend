using Application.Artigo.Contracts;
using MediatR;

namespace Application.Artigo.Queries;

/// <summary>
/// Lista artigos de um blog pelo ExternalId do blog (endpoint público).
/// </summary>
public record ListarArtigosPublicosPorBlogQuery(Guid BlogExternalId) : IRequest<IReadOnlyList<ArtigoPublicoResponse>?>;
