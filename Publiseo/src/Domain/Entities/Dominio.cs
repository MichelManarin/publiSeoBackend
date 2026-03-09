using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

/// <summary>
/// Domínio registrado pelo usuário na plataforma (compra via GoDaddy).
/// O domínio pertence ao usuário e pode ser utilizado em qualquer blog em que ele tenha vínculo.
/// </summary>
[ExcludeFromCodeCoverage]
public class Dominio
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    /// <summary>Nome do domínio (ex.: meusite.com).</summary>
    public string NomeDominio { get; set; } = string.Empty;
    public DateTime DataCompra { get; set; }
    /// <summary>Data em que o registro do domínio expira (DataCompra + Period anos).</summary>
    public DateTime DataExpiracao { get; set; }
    /// <summary>ID do pedido retornado pela GoDaddy (ou outro provedor).</summary>
    public long? OrdemIdExterno { get; set; }
    /// <summary>Valor total pago na compra.</summary>
    public decimal? Total { get; set; }
    /// <summary>Moeda (ex.: USD, BRL).</summary>
    public string? Moeda { get; set; }
    /// <summary>Período contratado em anos (ex.: 1, 2).</summary>
    public int PeriodoAnos { get; set; }
    /// <summary>Se privacidade WHOIS foi contratada.</summary>
    public bool Privacy { get; set; }
    /// <summary>Se renovação automática está ativa.</summary>
    public bool RenewAuto { get; set; }

    public Usuario Usuario { get; set; } = null!;
}
