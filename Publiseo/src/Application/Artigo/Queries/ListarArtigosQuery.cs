using Application.Artigo.Contracts;
using MediatR;

namespace Application.Artigo.Queries;

public record ListarArtigosQuery(Guid BlogId) : IRequest<IReadOnlyList<ArtigoResponse>>;
