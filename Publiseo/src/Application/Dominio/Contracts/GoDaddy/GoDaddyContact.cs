using System.Text.Json.Serialization;

namespace Application.Dominio.Contracts.GoDaddy;

/// <summary>Contato no formato esperado pela API GoDaddy (registrant, admin, billing, tech).</summary>
public class GoDaddyContact
{
    [JsonPropertyName("nameFirst")]
    public string NameFirst { get; set; } = string.Empty;

    [JsonPropertyName("nameLast")]
    public string NameLast { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonPropertyName("addressMailing")]
    public GoDaddyAddressMailing AddressMailing { get; set; } = new();
}
