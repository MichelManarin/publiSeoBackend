namespace Application.SearchConsole.Contracts;

/// <summary>
/// Métricas do Search Console armazenadas (por domínio e dia).
/// </summary>
public record SearchConsoleMetricaResponse(
    Guid Id,
    Guid BlogDominioId,
    string NomeDominio,
    DateOnly Data,
    string TipoBusca,
    long Impressoes,
    long Cliques,
    double Ctr,
    double PosicaoMedia,
    DateTime DataSincronizacao);
