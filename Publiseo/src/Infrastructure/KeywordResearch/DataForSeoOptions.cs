using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.KeywordResearch;

[ExcludeFromCodeCoverage]
public class DataForSeoOptions
{
    public const string SectionName = "DataForSeo";

    /// <summary>Login (e-mail) da conta em app.dataforseo.com.</summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>Password da API em app.dataforseo.com (não a senha da conta).</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>URL base da API (default: https://api.dataforseo.com).</summary>
    public string BaseUrl { get; set; } = "https://api.dataforseo.com";

    /// <summary>Código de local padrão (ex.: 2076 = Brasil).</summary>
    public int DefaultLocationCode { get; set; } = 2076;

    /// <summary>Código de idioma padrão (ex.: pt).</summary>
    public string DefaultLanguageCode { get; set; } = "pt";
}
