using Application.KeywordResearch.Abstractions;
using Application.KeywordResearch.Contracts;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.KeywordResearch.Queries;

[ExcludeFromCodeCoverage]
public sealed class PesquisarKeywordsQueryHandler : IRequestHandler<PesquisarKeywordsQuery, KeywordSearchPaginatedResponse?>
{
    private readonly IKeywordResearchAdapter _adapter;

    public PesquisarKeywordsQueryHandler(IKeywordResearchAdapter adapter) => _adapter = adapter;

    public async Task<KeywordSearchPaginatedResponse?> Handle(PesquisarKeywordsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Keyword))
            return null;

        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var offset = (page - 1) * pageSize;

        var result = await _adapter.SearchKeywordsAsync(
            request.Keyword.Trim(),
            request.LocationCode,
            request.LanguageCode,
            offset,
            pageSize,
            cancellationToken);

        if (result == null)
            return null;

        return new KeywordSearchPaginatedResponse(
            result.SeedKeyword,
            _adapter.SourceName,
            result.TotalCount,
            page,
            pageSize,
            result.Items);
    }
}
