using Application.KeywordResearch.Contracts;
using MediatR;

namespace Application.KeywordResearch.Queries;

/// <summary>
/// Pesquisa keywords (volume, concorrência, CPC, etc.) com resultado paginado.
/// </summary>
public record PesquisarKeywordsQuery(
    string Keyword,
    int Page = 1,
    int PageSize = 20,
    int? LocationCode = null,
    string? LanguageCode = null) : IRequest<KeywordSearchPaginatedResponse?>;
