using Application.SearchConsole.Contracts;
using MediatR;

namespace Application.SearchConsole.Queries;

/// <summary>
/// Lista métricas do Search Console consolidadas por data (todos os blogs do usuário).
/// </summary>
public record ListarMetricasSearchConsoleConsolidadasQuery(
    Guid UsuarioId,
    DateOnly DataInicio,
    DateOnly DataFim) : IRequest<IReadOnlyList<SearchConsoleMetricaConsolidadaResponse>>;
