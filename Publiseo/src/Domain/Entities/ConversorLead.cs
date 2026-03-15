using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

/// <summary>
/// Lead capturado quando o usuário completa o fluxo do conversor.
/// </summary>
[ExcludeFromCodeCoverage]
public class ConversorLead
{
    public Guid Id { get; set; }
    public Guid ConversorId { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    /// <summary>Respostas na ordem das perguntas (JSON array de strings ou texto).</summary>
    public string RespostasJson { get; set; } = "[]";
    /// <summary>Artigo em que estava quando preencheu (opcional).</summary>
    public Guid? ArtigoId { get; set; }
    public string? Ip { get; set; }
    public DateTime DataCriacao { get; set; }

    public Conversor Conversor { get; set; } = null!;
}
