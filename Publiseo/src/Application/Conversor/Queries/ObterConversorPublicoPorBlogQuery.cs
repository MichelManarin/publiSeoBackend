using Application.Conversor.Contracts;
using MediatR;

namespace Application.Conversor.Queries;

public record ObterConversorPublicoPorBlogQuery(Guid BlogExternalId) : IRequest<ConversorPublicoResponse?>;
