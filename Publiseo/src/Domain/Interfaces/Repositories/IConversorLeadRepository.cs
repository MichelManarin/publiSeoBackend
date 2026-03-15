using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IConversorLeadRepository
{
    Task<ConversorLead> InserirAsync(ConversorLead lead, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ConversorLead>> ListarPorBlogIdAsync(Guid blogId, CancellationToken cancellationToken = default);
}
