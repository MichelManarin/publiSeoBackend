using Application.Abstractions;
using Application.SearchConsole.Commands;
using Application.SearchConsole.Contracts;
using Microsoft.Extensions.Logging;
using Quartz;

namespace API.Jobs;

/// <summary>
/// Job Quartz que sincroniza métricas do Google Search Console (impressões, cliques, CTR, posição média) uma vez por dia.
/// Busca os dados do dia anterior para todos os domínios cadastrados e persiste no banco.
/// </summary>
public class SincronizarSearchConsoleJob : IJob
{
    private readonly IApplicationMediator _mediator;
    private readonly ILogger<SincronizarSearchConsoleJob> _logger;

    public SincronizarSearchConsoleJob(
        IApplicationMediator mediator,
        ILogger<SincronizarSearchConsoleJob> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Iniciando job de sincronização Search Console.");
        var result = await _mediator.Send(new SincronizarSearchConsoleCommand(), context.CancellationToken);
        _logger.LogInformation(
            "Sincronização Search Console concluída. Domínios={Dominios}, Métricas={Metricas}, Falhas={Falhas}.",
            result.DominiosProcessados, result.MetricasInseridasOuAtualizadas, result.Falhas);
    }
}
