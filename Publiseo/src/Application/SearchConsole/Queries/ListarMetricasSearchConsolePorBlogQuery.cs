using Application.SearchConsole.Contracts;
using MediatR;

namespace Application.SearchConsole.Queries;

/// <summary>
/// Lista métricas do Search Console por blog e período (dados já sincronizados no banco).
/// Requer que o usuário tenha acesso ao blog (dono ou membro).
/// </summary>
public record ListarMetricasSearchConsolePorBlogQuery(
    Guid UsuarioId,
    Guid BlogId,
    DateOnly DataInicio,
    DateOnly DataFim) : IRequest<IReadOnlyList<SearchConsoleMetricaResponse>?>;
