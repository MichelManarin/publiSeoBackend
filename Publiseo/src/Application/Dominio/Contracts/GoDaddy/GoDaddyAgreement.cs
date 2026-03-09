using System.Text.Json.Serialization;

namespace Application.Dominio.Contracts.GoDaddy;

/// <summary>Item retornado por GET /v1/domains/agreements.</summary>
public class GoDaddyAgreement
{
    [JsonPropertyName("agreementKey")]
    public string? AgreementKey { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}
