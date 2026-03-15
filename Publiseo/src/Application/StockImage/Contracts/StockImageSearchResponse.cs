namespace Application.StockImage.Contracts;

/// <summary>
/// Resposta da busca de imagens de estoque (qualquer provedor).
/// </summary>
public record StockImageSearchResponse(
    int Total,
    int TotalPages,
    IReadOnlyList<StockImageSearchResult> Results,
    string SourceName);
