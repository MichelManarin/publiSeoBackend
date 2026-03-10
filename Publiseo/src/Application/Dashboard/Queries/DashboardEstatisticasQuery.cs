using Application.Dashboard.Contracts;
using MediatR;

namespace Application.Dashboard.Queries;

/// <summary>
/// Obtém as estatísticas do dashboard do usuário (quantidade de blogs e de artigos).
/// </summary>
public record DashboardEstatisticasQuery(Guid UsuarioId) : IRequest<DashboardEstatisticasResponse>;
