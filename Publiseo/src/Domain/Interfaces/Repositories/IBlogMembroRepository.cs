using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IBlogMembroRepository
{
    Task InserirAsync(BlogMembro membro, CancellationToken cancellationToken = default);
    Task<bool> UsuarioEhMembroAsync(Guid blogId, Guid usuarioId, CancellationToken cancellationToken = default);
}
