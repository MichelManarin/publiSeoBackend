using Application.BlogIntegracao.Contracts;
using MediatR;

namespace Application.BlogIntegracao.Queries;

public record ListarIntegracoesPorBlogQuery(Guid UsuarioId, Guid BlogId) : IRequest<IReadOnlyList<BlogIntegracaoItemDto>>;
