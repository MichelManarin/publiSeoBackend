using Domain.Enums;

namespace Application.Conversor.Contracts;

public record ConversorPerguntaItemDto(
    Guid Id,
    int Ordem,
    string TextoPergunta,
    TipoCampoPergunta TipoCampo);
