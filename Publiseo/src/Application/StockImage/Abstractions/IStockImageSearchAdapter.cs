using Application.StockImage.Contracts;

namespace Application.StockImage.Abstractions;

/// <summary>
/// Adapter genérico para busca de imagens de estoque (Unsplash, Pexels, etc.).
/// Troque a implementação no DI para mudar o provedor.
/// </summary>
public interface IStockImageSearchAdapter
{
    /// <summary>
    /// Nome do provedor (ex.: "Unsplash", "Pexels").
    /// </summary>
    string SourceName { get; }

    /// <summary>
    /// Busca imagens por termo. Retorna URLs com parâmetros de atribuição quando aplicável.
    /// </summary>
    Task<StockImageSearchResponse?> SearchAsync(string query, int page = 1, int perPage = 10, CancellationToken cancellationToken = default);
}
