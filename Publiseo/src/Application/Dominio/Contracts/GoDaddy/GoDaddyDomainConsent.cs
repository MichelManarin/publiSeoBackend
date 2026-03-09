using System.Text.Json.Serialization;

namespace Application.Dominio.Contracts.GoDaddy;

/// <summary>Consentimento com os termos exigidos pelo TLD (agreementKeys retornados por /v1/domains/agreements).</summary>
public class GoDaddyDomainConsent
{
    [JsonPropertyName("agreementKeys")]
    public IReadOnlyList<string> AgreementKeys { get; set; } = Array.Empty<string>();

    [JsonPropertyName("agreedBy")]
    public string AgreedBy { get; set; } = string.Empty;

    [JsonPropertyName("agreedAt")]
    public string AgreedAt { get; set; } = string.Empty;
}
