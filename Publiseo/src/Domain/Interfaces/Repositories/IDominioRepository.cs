using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IDominioRepository
{
    Task<Dominio?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Dominio?> ObterPorUsuarioENomeAsync(Guid usuarioId, string nomeDominio, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Dominio>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<Dominio> InserirAsync(Dominio dominio, CancellationToken cancellationToken = default);
}
