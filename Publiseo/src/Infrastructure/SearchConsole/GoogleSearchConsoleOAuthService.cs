using System.Text.Json;
using Application.SearchConsole.Abstractions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
namespace Infrastructure.SearchConsole;

[ExcludeFromCodeCoverage]
public sealed class GoogleSearchConsoleOAuthService : IGoogleSearchConsoleOAuthService
{
    private const string Scope = "https://www.googleapis.com/auth/webmasters.readonly";
    private const string UserInfoEmailScope = "https://www.googleapis.com/auth/userinfo.email";
    private readonly SearchConsoleOptions _options;

    public GoogleSearchConsoleOAuthService(IOptions<SearchConsoleOptions> options)
    {
        _options = options.Value;
    }

    public string BuildAuthorizationUrl(string state)
    {
        var redirectUri = Uri.EscapeDataString(_options.OAuthRedirectUri.Trim());
        var scope = Uri.EscapeDataString(Scope + " " + UserInfoEmailScope);
        var clientId = Uri.EscapeDataString(_options.OAuthClientId);
        var stateEnc = Uri.EscapeDataString(state);
        return "https://accounts.google.com/o/oauth2/v2/auth?" +
               "response_type=code" +
               "&client_id=" + clientId +
               "&redirect_uri=" + redirectUri +
               "&scope=" + scope +
               "&access_type=offline" +
               "&prompt=consent" +
               "&state=" + stateEnc;
    }

    public async Task<(string RefreshToken, string? Email)> ExchangeCodeForTokenAsync(string code, string redirectUri, CancellationToken cancellationToken = default)
    {
        var flow = CreateFlow(redirectUri);
        var tokenResponse = await flow.ExchangeCodeForTokenAsync(
            "publiseo-user",
            code,
            redirectUri.Trim(),
            cancellationToken);

        var refreshToken = tokenResponse.RefreshToken ?? throw new InvalidOperationException("Google não retornou refresh token. Verifique prompt=consent na URL de autorização.");
        var email = await ObterEmailDoTokenAsync(tokenResponse.AccessToken, cancellationToken);
        return (refreshToken, email);
    }

    private GoogleAuthorizationCodeFlow CreateFlow(string redirectUri)
    {
        return new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _options.OAuthClientId,
                ClientSecret = _options.OAuthClientSecret
            },
            Scopes = new[] { Scope, UserInfoEmailScope },
            DataStore = new MemoryDataStore()
        });
    }

    private static async Task<string?> ObterEmailDoTokenAsync(string? accessToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(accessToken)) return null;
        try
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var response = await http.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo", cancellationToken);
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var email = JsonSerializer.Deserialize<JsonElement>(json);
            return email.TryGetProperty("email", out var prop) ? prop.GetString() : null;
        }
        catch
        {
            return null;
        }
    }

    private sealed class MemoryDataStore : IDataStore
    {
        public Task StoreAsync<T>(string key, T value) => Task.CompletedTask;
        public Task DeleteAsync<T>(string key) => Task.CompletedTask;
        public Task<T?> GetAsync<T>(string key) => Task.FromResult<T?>(default);
        public Task ClearAsync() => Task.CompletedTask;
    }
}
