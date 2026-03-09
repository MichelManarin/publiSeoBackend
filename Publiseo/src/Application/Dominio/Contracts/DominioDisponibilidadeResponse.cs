namespace Application.Dominio.Contracts;

public record DominioDisponibilidadeResponse
{
    public string Dominio { get; init; } = string.Empty;
    public bool Disponivel { get; init; }
    public string? Moeda { get; init; }
    public decimal? Preco { get; init; }
    public int? PeriodoAnos { get; init; }
    public bool Definitive { get; init; }
}
