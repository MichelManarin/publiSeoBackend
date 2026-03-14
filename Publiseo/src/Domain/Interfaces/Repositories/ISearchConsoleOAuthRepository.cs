using Domain.Entities;

namespace Domain.Interfaces.Repositories;

/// <summary>
/// Repositório do vínculo OAuth do usuário com o Google Search Console.
/// </summary>
public interface ISearchConsoleOAuthRepository
{
    Task<SearchConsoleOAuth?> ObterPorUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    /// <summary>Lista usuários que possuem GSC conectado (para o job de sincronização).</summary>
    Task<IReadOnlyList<SearchConsoleOAuth>> ListarTodosComTokenAsync(CancellationToken cancellationToken = default);
    Task<SearchConsoleOAuth> InserirOuAtualizarAsync(SearchConsoleOAuth oauth, CancellationToken cancellationToken = default);
    Task RemoverPorUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default);
}
