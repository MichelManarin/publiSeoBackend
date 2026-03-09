using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Usuario?> ObterPorLoginAsync(string login, CancellationToken cancellationToken = default);
    Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Usuario> InserirAsync(Usuario usuario, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Usuario usuario, CancellationToken cancellationToken = default);
}
