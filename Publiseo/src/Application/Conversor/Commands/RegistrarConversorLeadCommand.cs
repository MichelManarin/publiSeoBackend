using MediatR;

namespace Application.Conversor.Commands;

public record RegistrarConversorLeadCommand(
    Guid BlogExternalId,
    string NomeCompleto,
    string Telefone,
    IReadOnlyList<string> Respostas,
    Guid? ArtigoId,
    string? Ip) : IRequest<bool>;
