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
    private const string LogPrefix = "[INTEGRAÇÃO OPEN AI]";
    private const string PromptResourceName = "Infrastructure.Prompts.gerar-artigo-seo.txt";
    private const int DefaultNumeroPalavras = 500;
    private const string OpenAiUrl = "https://api.openai.com/v1/chat/completions";

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
            _logger.LogWarning("{Prefix} ApiKey não configurada. Configure OpenAI:ApiKey no appsettings.", LogPrefix);
            return null;
        }

        var prompt = await CarregarPromptAsync(cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrEmpty(prompt))
        {
            _logger.LogError("{Prefix} Erro ao carregar template do prompt (recurso: {Resource}).", LogPrefix, PromptResourceName);
            return null;
        }

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

        _logger.LogInformation("{Prefix} Enviando requisição para OpenAI. Modelo: {Model}, Título: {Titulo}.", LogPrefix, _options.Model, titulo);

        try
        {
            var response = await _httpClient.PostAsync(OpenAiUrl, content, cancellationToken).ConfigureAwait(false);
            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("{Prefix} Erro. StatusCode: {StatusCode}, Resposta: {Resposta}, Título: {Titulo}",
                    LogPrefix, (int)response.StatusCode, responseJson.Length > 500 ? responseJson[..500] + "..." : responseJson, titulo);
                return null;
            }

            var doc = JsonDocument.Parse(responseJson);
            var choices = doc.RootElement.GetProperty("choices");
            var first = choices.EnumerateArray().FirstOrDefault();
            if (first.ValueKind == JsonValueKind.Undefined)
            {
                _logger.LogWarning("{Prefix} Resposta sem choices. Resposta (início): {Resposta}, Título: {Titulo}",
                    LogPrefix, responseJson.Length > 300 ? responseJson[..300] + "..." : responseJson, titulo);
                return null;
            }

            var message = first.GetProperty("message").GetProperty("content").GetString();
            var resultado = message?.Trim();
            if (string.IsNullOrEmpty(resultado))
            {
                _logger.LogWarning("{Prefix} Conteúdo retornado vazio. Título: {Titulo}.", LogPrefix, titulo);
                return null;
            }

            _logger.LogInformation("{Prefix} Conteúdo gerado com sucesso. Título: {Titulo}, Tamanho: {Tamanho} chars.", LogPrefix, titulo, resultado.Length);
            return resultado;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "{Prefix} Erro de rede ao chamar OpenAI. Título: {Titulo}, Message: {Message}", LogPrefix, titulo, ex.Message);
            return null;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "{Prefix} Requisição cancelada (timeout ou cancelamento). Título: {Titulo}.", LogPrefix, titulo);
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "{Prefix} Erro ao interpretar resposta JSON da OpenAI. Título: {Titulo}.", LogPrefix, titulo);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Prefix} Erro. Título: {Titulo}, Tipo: {Tipo}, Message: {Message}",
                LogPrefix, titulo, ex.GetType().Name, ex.Message);
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
