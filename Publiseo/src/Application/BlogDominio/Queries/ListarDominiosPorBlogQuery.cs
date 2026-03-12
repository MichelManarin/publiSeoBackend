using Application.BlogDominio.Contracts;
using MediatR;

namespace Application.BlogDominio.Queries;

/// <summary>
/// Lista domínios vinculados a um blog. Apenas se o usuário tiver acesso ao blog (dono ou membro).
/// </summary>
public record ListarDominiosPorBlogQuery(Guid UsuarioId, Guid BlogId) : IRequest<IReadOnlyList<BlogDominioResponse>?>;
