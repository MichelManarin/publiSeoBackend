using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IArtigoRepository
{
    Task<Artigo?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Artigo>> ListarPorBlogAsync(Guid blogId, CancellationToken cancellationToken = default);
    Task<Artigo> InserirAsync(Artigo artigo, CancellationToken cancellationToken = default);
    Task<Artigo> AtualizarAsync(Artigo artigo, CancellationToken cancellationToken = default);
}
