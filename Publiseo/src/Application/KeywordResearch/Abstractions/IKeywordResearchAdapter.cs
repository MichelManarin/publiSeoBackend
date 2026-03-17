using Application.KeywordResearch.Contracts;

namespace Application.KeywordResearch.Abstractions;

/// <summary>
/// Adapter genérico para pesquisa de keywords (volume, concorrência, CPC, etc.).
/// Troque a implementação no DI para mudar o provedor (ex.: Data for SEO, SEMrush).
/// </summary>
public interface IKeywordResearchAdapter
{
    /// <summary>
    /// Nome do provedor (ex.: "DataForSEO", "SEMrush").
    /// </summary>
    string SourceName { get; }

    /// <summary>
    /// Busca keywords relacionadas e métricas. Suporta paginação via offset/limit.
    /// </summary>
    /// <param name="keyword">Palavra-chave de pesquisa.</param>
    /// <param name="locationCode">Código do local (ex.: 2076 = Brasil). Null usa padrão do provedor.</param>
    /// <param name="languageCode">Código do idioma (ex.: "pt"). Null usa padrão do provedor.</param>
    /// <param name="offset">Deslocamento para paginação (0-based).</param>
    /// <param name="limit">Quantidade de itens por página (ex.: 20).</param>
    Task<KeywordResearchResultDto?> SearchKeywordsAsync(
        string keyword,
        int? locationCode = null,
        string? languageCode = null,
        int offset = 0,
        int limit = 20,
        CancellationToken cancellationToken = default);
}
