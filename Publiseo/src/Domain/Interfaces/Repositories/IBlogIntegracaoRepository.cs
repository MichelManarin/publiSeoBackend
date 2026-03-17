using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IBlogIntegracaoRepository
{
    Task<BlogIntegracao?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlogIntegracao>> ListarPorBlogIdAsync(Guid blogId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlogIntegracao>> ListarPorBlogExternalIdAsync(Guid blogExternalId, CancellationToken cancellationToken = default);
    Task<BlogIntegracao> InserirAsync(BlogIntegracao integracao, CancellationToken cancellationToken = default);
    Task<BlogIntegracao> AtualizarAsync(BlogIntegracao integracao, CancellationToken cancellationToken = default);
    Task ExcluirAsync(BlogIntegracao integracao, CancellationToken cancellationToken = default);
}
