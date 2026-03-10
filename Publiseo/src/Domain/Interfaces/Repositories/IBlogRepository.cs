using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IBlogRepository
{
    Task<Blog?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Blog?> ObterPorExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Blog>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<Blog> InserirAsync(Blog blog, CancellationToken cancellationToken = default);
}
