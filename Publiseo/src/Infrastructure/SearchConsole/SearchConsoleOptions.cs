using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.SearchConsole;

[ExcludeFromCodeCoverage]
public class SearchConsoleOptions
{
    public const string SectionName = "SearchConsole";

    /// <summary>
    /// Caminho para o arquivo JSON da Service Account do Google (credenciais do GSC).
    /// O e-mail da service account deve ser adicionado como usuário em cada property no Search Console.
    /// </summary>
    public string CredentialJsonPath { get; set; } = string.Empty;

    /// <summary>
    /// Nome da aplicação enviado à API Google (opcional).
    /// </summary>
    public string ApplicationName { get; set; } = "Publiseo";

    // --- OAuth (conexão do usuário com a conta Google) ---

    /// <summary>Client ID do projeto Google Cloud (OAuth 2.0).</summary>
    public string OAuthClientId { get; set; } = string.Empty;
    /// <summary>Client Secret do projeto Google Cloud (OAuth 2.0).</summary>
    public string OAuthClientSecret { get; set; } = string.Empty;
    /// <summary>URL de callback após autorização (ex.: https://app.publiseo.com.br/api/searchconsole/callback).</summary>
    public string OAuthRedirectUri { get; set; } = string.Empty;
    /// <summary>URL para redirecionar após conectar com sucesso (ex.: https://app.publiseo.com.br/dashboard?gsc=connected).</summary>
    public string OAuthFrontendSuccessUrl { get; set; } = "/dashboard?gsc=connected";
    /// <summary>URL para redirecionar em caso de erro (ex.: https://app.publiseo.com.br/dashboard?gsc=error).</summary>
    public string OAuthFrontendErrorUrl { get; set; } = "/dashboard?gsc=error";
}
