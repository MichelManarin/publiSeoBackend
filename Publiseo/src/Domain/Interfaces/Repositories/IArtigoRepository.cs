using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IArtigoRepository
{
    Task<Artigo?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> ContarPorBlogIdsAsync(IReadOnlyList<Guid> blogIds, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Artigo>> ListarPorBlogAsync(Guid blogId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Artigo>> ListarPendentesGeracaoAsync(int maxTentativas, CancellationToken cancellationToken = default);
    Task<Artigo> InserirAsync(Artigo artigo, CancellationToken cancellationToken = default);
    Task<Artigo> AtualizarAsync(Artigo artigo, CancellationToken cancellationToken = default);
    Task ExcluirAsync(Guid id, CancellationToken cancellationToken = default);
}
