using MediatR;

namespace Application.Artigo.Commands;

/// <summary>
/// Exclui um artigo. Apenas se o usuário tiver acesso ao blog ao qual o artigo pertence (dono ou membro).
/// </summary>
public record ExcluirArtigoCommand(Guid Id, Guid UsuarioId) : IRequest<bool>;
