using Application.StockImage.Abstractions;
using Application.StockImage.Contracts;
using Infrastructure.Unsplash;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.StockImage;

[ExcludeFromCodeCoverage]
public sealed class UnsplashStockImageAdapter : IStockImageSearchAdapter
{
    private const string SearchUrl = "https://api.unsplash.com/search/photos";
    private const string SourceNameValue = "Unsplash";
    private readonly HttpClient _httpClient;
    private readonly UnsplashOptions _options;
    private readonly ILogger<UnsplashStockImageAdapter> _logger;

    public UnsplashStockImageAdapter(
        HttpClient httpClient,
        IOptions<UnsplashOptions> options,
        ILogger<UnsplashStockImageAdapter> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public string SourceName => SourceNameValue;

    public async Task<StockImageSearchResponse?> SearchAsync(string query, int page = 1, int perPage = 10, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiAccessKey))
        {
            _logger.LogWarning("Unsplash: ApiAccessKey não configurado.");
            return null;
        }

        var perPageClamped = Math.Clamp(perPage, 1, 30);
        var queryString = $"query={Uri.EscapeDataString(query.Trim())}&page={page}&per_page={perPageClamped}";
        var url = $"{SearchUrl}?{queryString}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Authorization", $"Client-ID {_options.ApiAccessKey}");
        request.Headers.Add("Accept-Version", "v1");

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Unsplash: falha ao chamar a API de busca.");
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Unsplash: API retornou {StatusCode}.", response.StatusCode);
            return null;
        }

        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var apiResponse = await response.Content.ReadFromJsonAsync<UnsplashApiSearchResponse>(jsonOptions, cancellationToken);
        if (apiResponse?.Results == null)
            return null;

        var utmSuffix = $"utm_source={Uri.EscapeDataString(_options.UtmSource)}&utm_medium=referral";
        var results = apiResponse.Results.Select(r => MapToStockResult(r, utmSuffix)).ToList();

        return new StockImageSearchResponse(
            apiResponse.Total,
            apiResponse.Total_Pages,
            results,
            SourceNameValue);
    }

    private static StockImageSearchResult MapToStockResult(UnsplashApiPhotoResult r, string utmSuffix)
    {
        string AppendUtm(string url) => url.Contains("?") ? $"{url}&{utmSuffix}" : $"{url}?{utmSuffix}";

        var urls = new StockImageSearchResultUrls(r.Urls.Raw, r.Urls.Full, r.Urls.Regular, r.Urls.Small, r.Urls.Thumb);
        var urlsWithAttribution = new StockImageSearchResultUrls(
            AppendUtm(r.Urls.Raw),
            AppendUtm(r.Urls.Full),
            AppendUtm(r.Urls.Regular),
            AppendUtm(r.Urls.Small),
            AppendUtm(r.Urls.Thumb));

        var userName = r.User?.Name ?? r.User?.Username ?? "Unsplash";
        var attribution = $"Photo by {userName} on Unsplash";
        var profileUrl = r.User?.Links?.Html ?? "https://unsplash.com";

        return new StockImageSearchResult(
            r.Id,
            r.Description,
            r.Alt_Description,
            urls,
            urlsWithAttribution,
            userName,
            profileUrl,
            attribution,
            r.Links?.Html ?? $"https://unsplash.com/photos/{r.Id}",
            SourceNameValue);
    }

    private sealed class UnsplashApiSearchResponse
    {
        public int Total { get; set; }
        [JsonPropertyName("total_pages")]
        public int Total_Pages { get; set; }
        public List<UnsplashApiPhotoResult>? Results { get; set; }
    }

    private sealed class UnsplashApiPhotoResult
    {
        public string Id { get; set; } = "";
        public string? Description { get; set; }
        [JsonPropertyName("alt_description")]
        public string? Alt_Description { get; set; }
        public UnsplashApiUrls Urls { get; set; } = new();
        public UnsplashApiUser? User { get; set; }
        public UnsplashApiLinks? Links { get; set; }
    }

    private sealed class UnsplashApiUrls
    {
        public string Raw { get; set; } = "";
        public string Full { get; set; } = "";
        public string Regular { get; set; } = "";
        public string Small { get; set; } = "";
        public string Thumb { get; set; } = "";
    }

    private sealed class UnsplashApiUser
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public UnsplashApiUserLinks? Links { get; set; }
    }

    private sealed class UnsplashApiUserLinks
    {
        public string? Html { get; set; }
    }

    private sealed class UnsplashApiLinks
    {
        public string? Html { get; set; }
    }
}
