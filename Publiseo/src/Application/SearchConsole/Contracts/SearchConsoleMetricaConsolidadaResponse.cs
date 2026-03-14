namespace Application.SearchConsole.Contracts;

/// <summary>
/// Métricas do Search Console agregadas por data (todos os blogs/domínios do usuário).
/// </summary>
public record SearchConsoleMetricaConsolidadaResponse(
    DateOnly Data,
    string TipoBusca,
    long Impressoes,
    long Cliques,
    double Ctr,
    double PosicaoMedia);
