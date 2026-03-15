namespace Application.Conversor.Contracts;

public record RegistrarConversorLeadRequest(
    Guid BlogExternalId,
    string NomeCompleto,
    string Telefone,
    IReadOnlyList<string> Respostas,
    Guid? ArtigoId = null);
