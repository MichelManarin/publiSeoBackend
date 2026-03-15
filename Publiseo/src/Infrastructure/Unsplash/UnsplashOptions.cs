using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Unsplash;

[ExcludeFromCodeCoverage]
public class UnsplashOptions
{
    public const string SectionName = "Unsplash";

    /// <summary>ID da aplicação no painel Unsplash (informacional).</summary>
    public string ApplicationId { get; set; } = string.Empty;

    /// <summary>Access Key para chamadas à API (header Authorization: Client-ID &lt;key&gt;).</summary>
    public string ApiAccessKey { get; set; } = string.Empty;

    /// <summary>Secret Key (usado em fluxos OAuth, se necessário).</summary>
    public string ApiSecretKey { get; set; } = string.Empty;

    /// <summary>Nome usado em utm_source nas URLs de atribuição (ex.: Publiseo).</summary>
    public string UtmSource { get; set; } = "Publiseo";
}
