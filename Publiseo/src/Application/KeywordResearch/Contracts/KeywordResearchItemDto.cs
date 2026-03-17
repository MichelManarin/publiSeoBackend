namespace Application.KeywordResearch.Contracts;

/// <summary>
/// Um item de keyword retornado pela pesquisa (provider-agnóstico).
/// </summary>
public record KeywordResearchItemDto(
    string Keyword,
    int SearchVolume,
    decimal Competition,
    string? CompetitionLevel,
    decimal Cpc,
    decimal? LowTopOfPageBid,
    decimal? HighTopOfPageBid,
    int? KeywordDifficulty,
    string? SearchIntent,
    long? SerpResultsCount,
    int? TrendMonthly,
    int? TrendQuarterly,
    int? TrendYearly,
    IReadOnlyList<MonthlySearchVolumeDto>? MonthlySearches);

/// <summary>
/// Volume de busca por mês (para gráfico ou média).
/// </summary>
public record MonthlySearchVolumeDto(int Year, int Month, int SearchVolume);
