using Application.Conversor.Contracts;
using MediatR;

namespace Application.Conversor.Queries;

/// <summary>
/// Lista os leads capturados pelo conversor do blog. Requer que o usuário tenha acesso ao blog.
/// </summary>
public record ListarLeadsConversorPorBlogQuery(Guid UsuarioId, Guid BlogId) : IRequest<IReadOnlyList<ConversorLeadItemResponse>>;
