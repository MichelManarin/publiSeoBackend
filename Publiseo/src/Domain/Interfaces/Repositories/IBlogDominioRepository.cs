using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IBlogDominioRepository
{
    Task<BlogDominio?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BlogDominio?> ObterPorNomeDominioAsync(string nomeDominio, CancellationToken cancellationToken = default);
    Task<BlogDominio?> ObterPorBlogENomeAsync(Guid blogId, string nomeDominio, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlogDominio>> ListarPorBlogAsync(Guid blogId, CancellationToken cancellationToken = default);
    Task<BlogDominio> InserirAsync(BlogDominio blogDominio, CancellationToken cancellationToken = default);
}
