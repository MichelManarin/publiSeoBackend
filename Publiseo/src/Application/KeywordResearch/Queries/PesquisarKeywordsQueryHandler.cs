using Application.KeywordResearch.Abstractions;
using Application.KeywordResearch.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Application.KeywordResearch.Queries;

[ExcludeFromCodeCoverage]
public sealed class PesquisarKeywordsQueryHandler : IRequestHandler<PesquisarKeywordsQuery, KeywordSearchPaginatedResponse?>
{
    private readonly IKeywordResearchAdapter _adapter;
    private readonly ILogger<PesquisarKeywordsQueryHandler> _logger;

    public PesquisarKeywordsQueryHandler(
        IKeywordResearchAdapter adapter,
        ILogger<PesquisarKeywordsQueryHandler> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<KeywordSearchPaginatedResponse?> Handle(PesquisarKeywordsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Keyword))
        {
            _logger.LogWarning("KeywordResearch: keyword vazia, ignorando.");
            return null;
        }

        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var offset = (page - 1) * pageSize;

        _logger.LogInformation(
            "KeywordResearch: chamando adapter keyword={Keyword}, page={Page}, pageSize={PageSize}, offset={Offset}.",
            request.Keyword.Trim(), page, pageSize, offset);

        var result = await _adapter.SearchKeywordsAsync(
            request.Keyword.Trim(),
            request.LocationCode,
            request.LanguageCode,
            offset,
            pageSize,
            cancellationToken);

        if (result == null)
        {
            _logger.LogWarning("KeywordResearch: adapter retornou null para keyword={Keyword}. Verifique logs do DataForSEO.", request.Keyword.Trim());
            return null;
        }

        _logger.LogInformation(
            "KeywordResearch: sucesso keyword={Keyword}, totalCount={TotalCount}, items={ItemsCount}, page={Page}.",
            result.SeedKeyword, result.TotalCount, result.Items.Count, page);

        return new KeywordSearchPaginatedResponse(
            result.SeedKeyword,
            _adapter.SourceName,
            result.TotalCount,
            page,
            pageSize,
            result.Items);
    }
}
