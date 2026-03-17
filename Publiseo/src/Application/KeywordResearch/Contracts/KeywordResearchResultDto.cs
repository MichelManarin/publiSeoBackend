namespace Application.KeywordResearch.Contracts;

/// <summary>
/// Resultado bruto da pesquisa de keywords (retorno do adapter).
/// </summary>
public record KeywordResearchResultDto(
    string SeedKeyword,
    int TotalCount,
    IReadOnlyList<KeywordResearchItemDto> Items);
