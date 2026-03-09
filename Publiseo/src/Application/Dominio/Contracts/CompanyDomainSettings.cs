namespace Application.Dominio.Contracts;

/// <summary>
/// Configuração fixa da empresa/plataforma para contato técnico (contactTech) na compra de domínio.
/// Deve vir de settings (appsettings ou provedor de configuração), sem hardcode na regra de negócio.
/// </summary>
public class CompanyDomainSettings
{
    public string CompanyTechFirstName { get; set; } = string.Empty;
    public string CompanyTechLastName { get; set; } = string.Empty;
    public string CompanyTechEmail { get; set; } = string.Empty;
    public string CompanyTechPhone { get; set; } = string.Empty;
    public string CompanyAddress1 { get; set; } = string.Empty;
    public string? CompanyAddress2 { get; set; }
    public string CompanyCity { get; set; } = string.Empty;
    public string CompanyState { get; set; } = string.Empty;
    public string CompanyPostalCode { get; set; } = string.Empty;
    public string CompanyCountry { get; set; } = string.Empty;
}
