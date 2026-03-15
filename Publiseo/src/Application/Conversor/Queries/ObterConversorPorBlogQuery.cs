using Application.Conversor.Contracts;
using MediatR;

namespace Application.Conversor.Queries;

public record ObterConversorPorBlogQuery(Guid UsuarioId, Guid BlogId) : IRequest<ConversorConfigResponse?>;
