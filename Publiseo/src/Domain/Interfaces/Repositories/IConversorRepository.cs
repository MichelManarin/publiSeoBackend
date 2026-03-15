using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IConversorRepository
{
    Task<Conversor?> ObterPorBlogIdAsync(Guid blogId, CancellationToken cancellationToken = default);
    Task<Conversor?> ObterPorBlogIdComPerguntasAsync(Guid blogId, CancellationToken cancellationToken = default);
    Task<Conversor?> ObterPorBlogExternalIdAsync(Guid blogExternalId, CancellationToken cancellationToken = default);
    Task<Conversor> InserirAsync(Conversor conversor, CancellationToken cancellationToken = default);
    Task<Conversor> AtualizarAsync(Conversor conversor, CancellationToken cancellationToken = default);
}
