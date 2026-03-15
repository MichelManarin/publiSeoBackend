namespace Application.StockImage.Contracts;

/// <summary>
/// Um resultado de imagem de estoque (provider-agnóstico: Unsplash, Pexels, etc.).
/// </summary>
public record StockImageSearchResult(
    string Id,
    string? Description,
    string? AltDescription,
    StockImageSearchResultUrls Urls,
    StockImageSearchResultUrls UrlsWithAttribution,
    string AuthorName,
    string AuthorProfileUrl,
    string Attribution,
    string PhotoPageUrl,
    string SourceName);

/// <summary>
/// URLs da imagem em vários tamanhos.
/// </summary>
public record StockImageSearchResultUrls(
    string Raw,
    string Full,
    string Regular,
    string Small,
    string Thumb);
