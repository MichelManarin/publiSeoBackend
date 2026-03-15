using Domain.Enums;

namespace Application.Conversor.Contracts;

public record SalvarConversorRequest(
    bool Ativo,
    string? TextoBotaoInicial,
    TipoFinalizacaoConversor TipoFinalizacao,
    string? MensagemFinalizacao,
    string? WhatsAppNumero,
    string? WhatsAppTextoPreDefinido,
    IReadOnlyList<SalvarConversorPerguntaItemDto> Perguntas);

public record SalvarConversorPerguntaItemDto(
    Guid? Id,
    int Ordem,
    string TextoPergunta,
    TipoCampoPergunta TipoCampo);
