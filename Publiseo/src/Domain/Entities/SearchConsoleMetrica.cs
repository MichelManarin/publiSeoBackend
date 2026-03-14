using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

/// <summary>
/// Métricas agregadas do Google Search Console por domínio e dia (impressões, cliques, CTR, posição média).
/// Dados preenchidos por job de sincronização para evitar consultas frequentes à API.
/// </summary>
[ExcludeFromCodeCoverage]
public class SearchConsoleMetrica
{
    public Guid Id { get; set; }
    public Guid BlogDominioId { get; set; }
    /// <summary>Data dos dados (um registro por dia).</summary>
    public DateOnly Data { get; set; }
    /// <summary>Tipo de busca: web, image, video, etc.</summary>
    public string TipoBusca { get; set; } = "web";
    public long Impressoes { get; set; }
    public long Cliques { get; set; }
    /// <summary>Click-through rate (0 a 1).</summary>
    public double Ctr { get; set; }
    /// <summary>Posição média na busca.</summary>
    public double PosicaoMedia { get; set; }
    /// <summary>Quando esta linha foi sincronizada da API.</summary>
    public DateTime DataSincronizacao { get; set; }

    public BlogDominio BlogDominio { get; set; } = null!;
}
