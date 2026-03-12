using Application.Abstractions;
using Application.Artigo.Commands;
using Application.Artigo.Contracts;
using Microsoft.Extensions.Logging;
using Quartz;

namespace API.Jobs;

/// <summary>
/// Job Quartz que processa artigos com geração por IA pendente a cada 5 minutos.
/// </summary>
public class ProcessarArtigosPendentesJob : IJob
{
    private readonly IApplicationMediator _mediator;
    private readonly ILogger<ProcessarArtigosPendentesJob> _logger;

    public ProcessarArtigosPendentesJob(
        IApplicationMediator mediator,
        ILogger<ProcessarArtigosPendentesJob> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Iniciando job de processamento de artigos pendentes (IA).");
        var result = await _mediator.Send(new ProcessarArtigosPendentesCommand(), context.CancellationToken);
        _logger.LogInformation(
            "Job de artigos pendentes concluído. Processados: {Processados}, Sucesso: {Sucesso}, Falha: {Falha}.",
            result.Processados, result.Sucesso, result.Falha);
    }
}
