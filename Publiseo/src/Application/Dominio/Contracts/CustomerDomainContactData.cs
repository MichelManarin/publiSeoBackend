namespace Application.Dominio.Contracts;

/// <summary>
/// Dados do cliente/usuário para preenchimento dos contatos de domínio (registrant, admin, billing).
/// Preenchido a partir da entidade Usuario.
/// </summary>
public record CustomerDomainContactData
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Address1 { get; init; } = string.Empty;
    public string? Address2 { get; init; }
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string PostalCode { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
}
