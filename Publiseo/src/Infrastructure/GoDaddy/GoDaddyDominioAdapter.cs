using Application.Dominio.Adapters;
using Application.Dominio.Contracts;
using Application.Dominio.Contracts.GoDaddy;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;

namespace Infrastructure.GoDaddy;

[ExcludeFromCodeCoverage]
public sealed class GoDaddyDominioAdapter : IDominioAdapter
{
    private readonly HttpClient _httpClient;
    private readonly GoDaddyOptions _options;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public GoDaddyDominioAdapter(HttpClient httpClient, IOptions<GoDaddyOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<DominioDisponibilidadeResponse> VerificarDisponibilidadeAsync(string dominio, CancellationToken cancellationToken = default)
    {
        var domain = dominio.Trim().ToLowerInvariant();
        var url = $"{_options.BaseUrl.TrimEnd('/')}/v1/domains/available?domain={Uri.EscapeDataString(domain)}&checkType=FAST&forTransfer=false";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        AddAuth(request);
        request.Headers.Add("Accept", "application/json");

        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"GoDaddy API error: {response.StatusCode}. {body}");
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var dto = JsonSerializer.Deserialize<GoDaddyAvailableResponse>(json, JsonOptions);
        if (dto == null)
            return new DominioDisponibilidadeResponse { Dominio = domain, Disponivel = false };

        return new DominioDisponibilidadeResponse
        {
            Dominio = dto.Domain ?? domain,
            Disponivel = dto.Available,
            Moeda = dto.Currency,
            Preco = dto.Price.HasValue ? dto.Price.Value / 100m : null,
            PeriodoAnos = dto.Period,
            Definitive = dto.Definitive
        };
    }

    public async Task<IReadOnlyList<SugestaoDominioItemResponse>> SugerirAsync(
        SugestaoDominioOptions options,
        string? shopperId,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new List<string> { $"query={Uri.EscapeDataString(options.Query.Trim())}" };
        if (!string.IsNullOrWhiteSpace(options.Country))
            queryParams.Add($"country={Uri.EscapeDataString(options.Country.Trim())}");
        if (!string.IsNullOrWhiteSpace(options.City))
            queryParams.Add($"city={Uri.EscapeDataString(options.City.Trim())}");
        if (options.Sources?.Count > 0)
            foreach (var s in options.Sources)
                queryParams.Add($"sources={Uri.EscapeDataString(s)}");
        if (options.Tlds?.Count > 0)
            foreach (var t in options.Tlds)
                queryParams.Add($"tlds={Uri.EscapeDataString(t)}");
        if (options.LengthMin.HasValue)
            queryParams.Add($"lengthMin={options.LengthMin.Value}");
        if (options.LengthMax.HasValue)
            queryParams.Add($"lengthMax={options.LengthMax.Value}");
        if (options.Limit.HasValue)
            queryParams.Add($"limit={options.Limit.Value}");
        if (options.WaitMs.HasValue)
            queryParams.Add($"waitMs={options.WaitMs.Value}");

        var url = $"{_options.BaseUrl.TrimEnd('/')}/v1/domains/suggest?{string.Join("&", queryParams)}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        AddAuth(request);
        request.Headers.Add("Accept", "application/json");
        if (!string.IsNullOrWhiteSpace(shopperId))
            request.Headers.Add("X-Shopper-Id", shopperId);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"GoDaddy API error: {response.StatusCode}. {body}");
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var dtos = JsonSerializer.Deserialize<List<GoDaddySuggestItemDto>>(json, JsonOptions);
        if (dtos == null || dtos.Count == 0)
            return Array.Empty<SugestaoDominioItemResponse>();

        return dtos.Select(d => new SugestaoDominioItemResponse { Dominio = d.Domain ?? "" }).ToList();
    }

    public async Task<IReadOnlyList<GoDaddyAgreement>> ObterAgreementsAsync(string tld, bool privacy, CancellationToken cancellationToken = default)
    {
        var url = $"{_options.BaseUrl.TrimEnd('/')}/v1/domains/agreements?tlds={Uri.EscapeDataString(tld)}&privacy={privacy.ToString().ToLowerInvariant()}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        AddAuth(request);
        request.Headers.Add("Accept", "application/json");

        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"GoDaddy agreements error: {response.StatusCode}. {body}");
        }

        var list = await response.Content.ReadFromJsonAsync<List<GoDaddyAgreement>>(JsonOptions, cancellationToken);
        return list ?? new List<GoDaddyAgreement>();
    }

    public async Task ValidarCompraAsync(GoDaddyDomainPurchaseRequest request, CancellationToken cancellationToken = default)
    {
        var url = $"{_options.BaseUrl.TrimEnd('/')}/v1/domains/purchase/validate";
        using var content = JsonContent.Create(request, options: JsonOptions);
        var message = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
        AddAuth(message);
        message.Headers.Add("Accept", "application/json");

        var response = await _httpClient.SendAsync(message, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"GoDaddy validate purchase error: {response.StatusCode}. {body}");
        }
    }

    public async Task<GoDaddyDomainPurchaseResponse> ComprarAsync(GoDaddyDomainPurchaseRequest request, CancellationToken cancellationToken = default)
    {
        var url = $"{_options.BaseUrl.TrimEnd('/')}/v1/domains/purchase";
        using var content = JsonContent.Create(request, options: JsonOptions);
        var message = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
        AddAuth(message);
        message.Headers.Add("Accept", "application/json");

        var response = await _httpClient.SendAsync(message, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"GoDaddy purchase error: {response.StatusCode}. {body}");

        var result = JsonSerializer.Deserialize<GoDaddyDomainPurchaseResponse>(body, JsonOptions);
        if (result == null)
            throw new InvalidOperationException("Resposta inválida da GoDaddy ao comprar domínio.");
        return result;
    }

    private void AddAuth(HttpRequestMessage request) =>
        request.Headers.Add("Authorization", $"sso-key {_options.ApiKey}:{_options.ApiSecret}");

    private sealed class GoDaddyAvailableResponse
    {
        public bool Available { get; set; }
        public string? Currency { get; set; }
        public bool Definitive { get; set; }
        public string? Domain { get; set; }
        public int? Period { get; set; }
        public long? Price { get; set; }
    }

    private sealed class GoDaddySuggestItemDto
    {
        public string? Domain { get; set; }
    }
}
