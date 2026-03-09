using System.Text.Json.Serialization;

namespace Application.Dominio.Contracts.GoDaddy;

/// <summary>Endereço postal no formato esperado pela API GoDaddy (domains/purchase).</summary>
public class GoDaddyAddressMailing
{
    [JsonPropertyName("address1")]
    public string Address1 { get; set; } = string.Empty;

    [JsonPropertyName("address2")]
    public string Address2 { get; set; } = string.Empty;

    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; } = string.Empty;

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;
}
