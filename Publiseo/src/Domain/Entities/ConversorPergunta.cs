using System.Diagnostics.CodeAnalysis;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Pergunta do conversor, em ordem.
/// </summary>
[ExcludeFromCodeCoverage]
public class ConversorPergunta
{
    public Guid Id { get; set; }
    public Guid ConversorId { get; set; }
    public int Ordem { get; set; }
    public string TextoPergunta { get; set; } = string.Empty;
    public TipoCampoPergunta TipoCampo { get; set; }

    public Conversor Conversor { get; set; } = null!;
}
