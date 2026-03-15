using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IConversorLeadRepository
{
    Task<ConversorLead> InserirAsync(ConversorLead lead, CancellationToken cancellationToken = default);
}
