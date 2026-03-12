using MediatR;

namespace Application.BlogDominio.Commands;

/// <summary>
/// Remove um domínio do blog. Apenas se o usuário tiver acesso ao blog ao qual o domínio está vinculado.
/// </summary>
public record RemoverBlogDominioCommand(Guid UsuarioId, Guid BlogDominioId) : IRequest<bool>;
