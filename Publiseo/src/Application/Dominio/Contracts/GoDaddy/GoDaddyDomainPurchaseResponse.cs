using System.Text.Json.Serialization;

namespace Application.Dominio.Contracts.GoDaddy;

/// <summary>Resposta de sucesso do POST /v1/domains/purchase.</summary>
public class GoDaddyDomainPurchaseResponse
{
    [JsonPropertyName("orderId")]
    public long OrderId { get; set; }

    [JsonPropertyName("total")]
    public decimal Total { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }
}
