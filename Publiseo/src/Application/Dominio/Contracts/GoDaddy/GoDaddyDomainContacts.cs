using System.Text.Json.Serialization;

namespace Application.Dominio.Contracts.GoDaddy;

/// <summary>Conjunto de contatos para compra de domínio: registrante e admin = cliente; tech = empresa; billing = cliente (coerência cadastral).</summary>
public class GoDaddyDomainContacts
{
    [JsonPropertyName("contactRegistrant")]
    public GoDaddyContact ContactRegistrant { get; set; } = new();

    [JsonPropertyName("contactAdmin")]
    public GoDaddyContact ContactAdmin { get; set; } = new();

    [JsonPropertyName("contactBilling")]
    public GoDaddyContact ContactBilling { get; set; } = new();

    [JsonPropertyName("contactTech")]
    public GoDaddyContact ContactTech { get; set; } = new();
}
