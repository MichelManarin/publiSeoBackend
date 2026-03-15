using Domain.Enums;

namespace Application.Conversor.Contracts;

/// <summary>
/// Configuração pública do conversor (se ativo) para o front renderizar o widget.
/// </summary>
public record ConversorPublicoResponse(
    bool Ativo,
    string? TextoBotaoInicial,
    TipoFinalizacaoConversor TipoFinalizacao,
    string? MensagemFinalizacao,
    string? WhatsAppNumero,
    string? WhatsAppTextoPreDefinido,
    IReadOnlyList<ConversorPerguntaPublicoItemDto> Perguntas);

public record ConversorPerguntaPublicoItemDto(
    int Ordem,
    string TextoPergunta,
    TipoCampoPergunta TipoCampo);
