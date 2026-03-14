namespace Application.SearchConsole.Contracts;

/// <summary>
/// Resultado da sincronização de métricas do Search Console.
/// </summary>
public record SincronizarSearchConsoleResult(
    int DominiosProcessados,
    int MetricasInseridasOuAtualizadas,
    int Falhas);
