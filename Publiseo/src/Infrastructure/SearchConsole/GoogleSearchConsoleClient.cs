using Application.SearchConsole.Abstractions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Webmasters.v3;
using Google.Apis.Webmasters.v3.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.SearchConsole;

[ExcludeFromCodeCoverage]
public sealed class GoogleSearchConsoleClient : ISearchConsoleClient
{
    private const string Scope = "https://www.googleapis.com/auth/webmasters.readonly";
    private readonly SearchConsoleOptions _options;
    private readonly ILogger<GoogleSearchConsoleClient> _logger;

    public GoogleSearchConsoleClient(
        IOptions<SearchConsoleOptions> options,
        ILogger<GoogleSearchConsoleClient> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<SearchConsoleMetricasDto?> ObterMetricasAgregadasAsync(
        string siteUrl,
        DateOnly data,
        string tipoBusca = "web",
        string? refreshToken = null,
        CancellationToken cancellationToken = default)
    {
        ICredential? credential = !string.IsNullOrWhiteSpace(refreshToken)
            ? await CreateUserCredentialFromRefreshTokenAsync(refreshToken, cancellationToken)
            : await CreateServiceAccountCredentialAsync(cancellationToken);

        if (credential == null)
            return null;

        try
        {
            var service = new WebmastersService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = string.IsNullOrWhiteSpace(_options.ApplicationName) ? "Publiseo" : _options.ApplicationName
            });

            var siteUrlNormalizado = NormalizarSiteUrl(siteUrl);
            var request = new SearchAnalyticsQueryRequest
            {
                StartDate = data.ToString("yyyy-MM-dd"),
                EndDate = data.ToString("yyyy-MM-dd"),
                Dimensions = new List<string> { "date" },
                SearchType = tipoBusca
            };

            var response = await service.Searchanalytics.Query(request, siteUrlNormalizado).ExecuteAsync(cancellationToken);
            var row = response.Rows?.FirstOrDefault();
            if (row == null)
                return null;

            return new SearchConsoleMetricasDto(
                (long)(row.Impressions ?? 0),
                (long)(row.Clicks ?? 0),
                row.Ctr ?? 0,
                row.Position ?? 0);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Search Console: falha ao obter métricas para {SiteUrl} em {Data}.", siteUrl, data);
            throw;
        }
    }

    private async Task<ICredential?> CreateUserCredentialFromRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.OAuthClientId) || string.IsNullOrWhiteSpace(_options.OAuthClientSecret))
        {
            _logger.LogWarning("Search Console OAuth: ClientId/ClientSecret não configurados.");
            return null;
        }
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets { ClientId = _options.OAuthClientId, ClientSecret = _options.OAuthClientSecret },
            Scopes = new[] { Scope },
            DataStore = new MemoryDataStore()
        });
        var tokenResponse = new TokenResponse { RefreshToken = refreshToken };
        var credential = new UserCredential(flow, "publiseo", tokenResponse);
        await credential.RefreshTokenAsync(cancellationToken);
        return credential;
    }

    private async Task<ICredential?> CreateServiceAccountCredentialAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.CredentialJsonPath) || !File.Exists(_options.CredentialJsonPath))
        {
            _logger.LogWarning("Search Console: CredentialJsonPath não configurado ou arquivo não encontrado.");
            return null;
        }
        using var stream = new FileStream(_options.CredentialJsonPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var credential = await GoogleCredential.FromStreamAsync(stream, cancellationToken);
        return credential.CreateScoped(WebmastersService.Scope.WebmastersReadonly);
    }

    private sealed class MemoryDataStore : IDataStore
    {
        public Task StoreAsync<T>(string key, T value) => Task.CompletedTask;
        public Task DeleteAsync<T>(string key) => Task.CompletedTask;
        public Task<T?> GetAsync<T>(string key) => Task.FromResult<T?>(default);
        public Task ClearAsync() => Task.CompletedTask;
    }

    private static string NormalizarSiteUrl(string nomeOuUrl)
    {
        var s = nomeOuUrl.Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(s)) return s;
        if (s.StartsWith("sc-domain:", StringComparison.OrdinalIgnoreCase)) return s;
        if (s.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || s.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return s.EndsWith("/", StringComparison.Ordinal) ? s : s + "/";
        return "https://" + s + "/";
    }
}
