namespace Application.Dominio.Contracts;

/// <summary>Resposta da operação de compra de domínio (após sucesso na GoDaddy e persistência local).</summary>
public record ComprarDominioResponse
{
    public Guid DominioId { get; init; }
    public string NomeDominio { get; init; } = string.Empty;
    public long OrdemIdExterno { get; init; }
    public DateTime DataCompra { get; init; }
    public DateTime DataExpiracao { get; init; }
    public decimal? Total { get; init; }
    public string? Moeda { get; init; }
    public int PeriodoAnos { get; init; }
    public bool Privacy { get; init; }
    public bool RenewAuto { get; init; }
}
