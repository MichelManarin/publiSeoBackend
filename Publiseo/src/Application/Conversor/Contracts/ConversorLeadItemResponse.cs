namespace Application.Conversor.Contracts;

/// <summary>
/// Item de lead do conversor para listagem no painel.
/// </summary>
public record ConversorLeadItemResponse(
    Guid Id,
    string NomeCompleto,
    string Telefone,
    IReadOnlyList<string> Respostas,
    Guid? ArtigoId,
    string? Ip,
    DateTime DataCriacao);
