using Application.Artigo.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Linq;

namespace Infrastructure.OpenAI;

[ExcludeFromCodeCoverage]
public sealed class OpenAIGeradorConteudoArtigoService : IGeradorConteudoArtigoService
{
    private const string PromptResourceName = "Infrastructure.Prompts.gerar-artigo-seo.txt";
    private const int DefaultNumeroPalavras = 500;

    private readonly HttpClient _httpClient;
    private readonly OpenAIOptions _options;
    private readonly ILogger<OpenAIGeradorConteudoArtigoService> _logger;

    public OpenAIGeradorConteudoArtigoService(
        HttpClient httpClient,
        IOptions<OpenAIOptions> options,
        ILogger<OpenAIGeradorConteudoArtigoService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string?> GerarConteudoAsync(string titulo, int numeroPalavras, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            _logger.LogWarning("OpenAI ApiKey não configurada.");
            return null;
        }

        var prompt = await CarregarPromptAsync(cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrEmpty(prompt))
            return null;

        prompt = prompt
            .Replace("{{TITULO}}", titulo.Trim(), StringComparison.OrdinalIgnoreCase)
            .Replace("{{NUMERO_PALAVRAS}}", (numeroPalavras > 0 ? numeroPalavras : DefaultNumeroPalavras).ToString(), StringComparison.OrdinalIgnoreCase);

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _options.ApiKey);
        var request = new
        {
            model = _options.Model,
            messages = new[] { new { role = "user", content = prompt } },
            temperature = 0.7
        };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var doc = JsonDocument.Parse(responseJson);
            var choices = doc.RootElement.GetProperty("choices");
            var first = choices.EnumerateArray().FirstOrDefault();
            if (first.ValueKind == JsonValueKind.Undefined)
                return null;
            var message = first.GetProperty("message").GetProperty("content").GetString();
            return message?.Trim();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar conteúdo via OpenAI para título: {Titulo}", titulo);
            return null;
        }
    }

    private static async Task<string?> CarregarPromptAsync(CancellationToken cancellationToken)
    {
        var assembly = Assembly.GetExecutingAssembly();
        await using var stream = assembly.GetManifestResourceStream(PromptResourceName);
        if (stream == null)
            return null;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
    }
}
