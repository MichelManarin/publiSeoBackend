namespace Application.KeywordResearch.Contracts;

/// <summary>
/// Resposta paginada da pesquisa de keywords para o front.
/// </summary>
public record KeywordSearchPaginatedResponse(
    string SeedKeyword,
    string SourceName,
    int TotalCount,
    int Page,
    int PageSize,
    IReadOnlyList<KeywordResearchItemDto> Items);
