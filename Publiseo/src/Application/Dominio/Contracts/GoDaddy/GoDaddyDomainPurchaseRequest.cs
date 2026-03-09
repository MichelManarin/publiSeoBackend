using System.Text.Json.Serialization;

namespace Application.Dominio.Contracts.GoDaddy;

/// <summary>Payload completo para POST /v1/domains/purchase na API GoDaddy.</summary>
public class GoDaddyDomainPurchaseRequest
{
    [JsonPropertyName("domain")]
    public string Domain { get; set; } = string.Empty;

    [JsonPropertyName("consent")]
    public GoDaddyDomainConsent Consent { get; set; } = new();

    [JsonPropertyName("contactAdmin")]
    public GoDaddyContact ContactAdmin { get; set; } = new();

    [JsonPropertyName("contactBilling")]
    public GoDaddyContact ContactBilling { get; set; } = new();

    [JsonPropertyName("contactRegistrant")]
    public GoDaddyContact ContactRegistrant { get; set; } = new();

    [JsonPropertyName("contactTech")]
    public GoDaddyContact ContactTech { get; set; } = new();

    [JsonPropertyName("period")]
    public int Period { get; set; } = 1;

    [JsonPropertyName("privacy")]
    public bool Privacy { get; set; }

    [JsonPropertyName("renewAuto")]
    public bool RenewAuto { get; set; } = true;
}
