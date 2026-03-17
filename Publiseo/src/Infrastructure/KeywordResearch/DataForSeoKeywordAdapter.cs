using Application.KeywordResearch.Abstractions;
using Application.KeywordResearch.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.KeywordResearch;

[ExcludeFromCodeCoverage]
public sealed class DataForSeoKeywordAdapter : IKeywordResearchAdapter
{
    private const string SourceNameValue = "DataForSEO";
    private const string RelatedKeywordsPath = "v3/dataforseo_labs/google/related_keywords/live";
    private readonly HttpClient _httpClient;
    private readonly DataForSeoOptions _options;
    private readonly ILogger<DataForSeoKeywordAdapter> _logger;

    public DataForSeoKeywordAdapter(
        HttpClient httpClient,
        IOptions<DataForSeoOptions> options,
        ILogger<DataForSeoKeywordAdapter> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public string SourceName => SourceNameValue;

    public async Task<KeywordResearchResultDto?> SearchKeywordsAsync(
        string keyword,
        int? locationCode = null,
        string? languageCode = null,
        int offset = 0,
        int limit = 20,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.Login) || string.IsNullOrWhiteSpace(_options.Password))
        {
            _logger.LogWarning("DataForSEO: Login ou Password não configurado.");
            return null;
        }

        var loc = locationCode ?? _options.DefaultLocationCode;
        var lang = languageCode ?? _options.DefaultLanguageCode;
        var limitClamped = Math.Clamp(limit, 1, 1000);

        _logger.LogInformation(
            "DataForSEO: pesquisando keyword={Keyword}, offset={Offset}, limit={Limit}, locationCode={LocationCode}, languageCode={LanguageCode}.",
            keyword.Trim(), offset, limitClamped, loc, lang);

        var payload = new[] { new
        {
            keyword = keyword.Trim(),
            location_code = loc,
            language_code = lang,
            depth = 4,
            limit = limitClamped,
            offset,
            include_seed_keyword = true,
            include_serp_info = true,
            ignore_synonyms = false,
            include_clickstream_data = false,
            replace_with_core_keyword = false
        }};

        var request = new HttpRequestMessage(HttpMethod.Post, RelatedKeywordsPath);
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_options.Login}:{_options.Password}")));
        request.Content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "DataForSEO: falha ao chamar a API.");
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning(
                "DataForSEO: API retornou {StatusCode}. Body (primeiros 500 chars): {BodyPreview}",
                response.StatusCode,
                errorBody.Length > 500 ? errorBody[..500] : errorBody);
            return null;
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogDebug("DataForSEO: resposta OK, body length={Length}.", json.Length);

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        DataForSeoTasksWrapper? wrapper;
        try
        {
            wrapper = JsonSerializer.Deserialize<DataForSeoTasksWrapper>(json, jsonOptions);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "DataForSEO: falha ao deserializar JSON. Path: {Path}. Preview: {Preview}",
                ex.Path, json.Length > 300 ? json[..300] : json);
            return null;
        }

        var taskResult = wrapper?.Tasks?.FirstOrDefault();
        if (taskResult == null)
        {
            _logger.LogWarning("DataForSEO: tasks vazio ou null. Preview: {Preview}",
                json.Length > 300 ? json[..300] : json);
            return null;
        }

        if (taskResult.Result == null || taskResult.Result.Count == 0)
        {
            _logger.LogWarning("DataForSEO: result vazio. Task StatusCode={StatusCode}, StatusMessage={Message}.",
                taskResult.StatusCode, taskResult.StatusMessage);
            return null;
        }

        if (taskResult.StatusCode != 20000)
        {
            _logger.LogWarning("DataForSEO: task status {StatusCode} - {Message}.", taskResult.StatusCode, taskResult.StatusMessage);
            return null;
        }

        var result = taskResult.Result[0];
        var items = result.Items != null
            ? result.Items.Where(i => i.KeywordData != null).Select(i => MapItem(i.KeywordData!.Keyword, i.KeywordData)).ToList()
            : new List<KeywordResearchItemDto>();
        var totalCount = result.TotalCount;

        _logger.LogInformation(
            "DataForSEO: sucesso. SeedKeyword={SeedKeyword}, TotalCount={TotalCount}, ItemsRetornados={ItemsCount}.",
            result.SeedKeyword, totalCount, items.Count);

        return new KeywordResearchResultDto(result.SeedKeyword ?? keyword.Trim(), totalCount, items);
    }

    private static KeywordResearchItemDto MapItem(string keyword, DataForSeoKeywordDataDto data)
    {
        var info = data.KeywordInfo;
        var props = data.KeywordProperties;
        var serp = data.SerpInfo;
        var intent = data.SearchIntentInfo;
        var trend = info?.SearchVolumeTrend;

        return new KeywordResearchItemDto(
            keyword,
            info?.SearchVolume ?? 0,
            info?.Competition ?? 0,
            info?.CompetitionLevel,
            info?.Cpc ?? 0,
            info?.LowTopOfPageBid,
            info?.HighTopOfPageBid,
            props?.KeywordDifficulty,
            intent?.MainIntent,
            serp?.SeResultsCount,
            trend?.Monthly,
            trend?.Quarterly,
            trend?.Yearly,
            MapMonthlySearches(info?.MonthlySearches));
    }

    private static IReadOnlyList<MonthlySearchVolumeDto>? MapMonthlySearches(List<DataForSeoMonthlySearchDto>? list)
    {
        if (list == null || list.Count == 0) return null;
        return list
            .Where(m => m.Year.HasValue && m.Month.HasValue)
            .Select(m => new MonthlySearchVolumeDto(m.Year!.Value, m.Month!.Value, m.SearchVolume ?? 0))
            .ToList();
    }

    private sealed class DataForSeoTasksWrapper
    {
        [JsonPropertyName("tasks")]
        public List<DataForSeoTaskItem>? Tasks { get; set; }
    }

    private sealed class DataForSeoTaskItem
    {
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }
        [JsonPropertyName("status_message")]
        public string? StatusMessage { get; set; }
        [JsonPropertyName("result")]
        public List<DataForSeoResultItem>? Result { get; set; }
    }

    private sealed class DataForSeoResultItem
    {
        [JsonPropertyName("seed_keyword")]
        public string? SeedKeyword { get; set; }
        [JsonPropertyName("seed_keyword_data")]
        public DataForSeoKeywordDataDto? SeedKeywordData { get; set; }
        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }
        [JsonPropertyName("items")]
        public List<DataForSeoResultKeywordItem>? Items { get; set; }
    }

    private sealed class DataForSeoResultKeywordItem
    {
        [JsonPropertyName("keyword_data")]
        public DataForSeoKeywordDataDto? KeywordData { get; set; }
    }

    private sealed class DataForSeoKeywordDataDto
    {
        [JsonPropertyName("keyword")]
        public string Keyword { get; set; } = string.Empty;
        [JsonPropertyName("keyword_info")]
        public DataForSeoKeywordInfoDto? KeywordInfo { get; set; }
        [JsonPropertyName("keyword_properties")]
        public DataForSeoKeywordPropertiesDto? KeywordProperties { get; set; }
        [JsonPropertyName("serp_info")]
        public DataForSeoSerpInfoDto? SerpInfo { get; set; }
        [JsonPropertyName("search_intent_info")]
        public DataForSeoSearchIntentDto? SearchIntentInfo { get; set; }
    }

    private sealed class DataForSeoKeywordInfoDto
    {
        [JsonPropertyName("search_volume")]
        public int? SearchVolume { get; set; }
        [JsonPropertyName("competition")]
        public decimal? Competition { get; set; }
        [JsonPropertyName("competition_level")]
        public string? CompetitionLevel { get; set; }
        [JsonPropertyName("cpc")]
        public decimal? Cpc { get; set; }
        [JsonPropertyName("low_top_of_page_bid")]
        public decimal? LowTopOfPageBid { get; set; }
        [JsonPropertyName("high_top_of_page_bid")]
        public decimal? HighTopOfPageBid { get; set; }
        [JsonPropertyName("monthly_searches")]
        public List<DataForSeoMonthlySearchDto>? MonthlySearches { get; set; }
        [JsonPropertyName("search_volume_trend")]
        public DataForSeoTrendDto? SearchVolumeTrend { get; set; }
    }

    private sealed class DataForSeoMonthlySearchDto
    {
        [JsonPropertyName("year")]
        public int? Year { get; set; }
        [JsonPropertyName("month")]
        public int? Month { get; set; }
        [JsonPropertyName("search_volume")]
        public int? SearchVolume { get; set; }
    }

    private sealed class DataForSeoTrendDto
    {
        [JsonPropertyName("monthly")]
        public int? Monthly { get; set; }
        [JsonPropertyName("quarterly")]
        public int? Quarterly { get; set; }
        [JsonPropertyName("yearly")]
        public int? Yearly { get; set; }
    }

    private sealed class DataForSeoKeywordPropertiesDto
    {
        [JsonPropertyName("keyword_difficulty")]
        public int? KeywordDifficulty { get; set; }
    }

    private sealed class DataForSeoSerpInfoDto
    {
        [JsonPropertyName("se_results_count")]
        public long? SeResultsCount { get; set; }
    }

    private sealed class DataForSeoSearchIntentDto
    {
        [JsonPropertyName("main_intent")]
        public string? MainIntent { get; set; }
    }
}
