using Domain.Enums;

namespace Application.Conversor.Contracts;

/// <summary>
/// Configuração do conversor para o painel (GET/PUT por blogId).
/// </summary>
public record ConversorConfigResponse(
    Guid Id,
    Guid BlogId,
    bool Ativo,
    string? TextoBotaoInicial,
    TipoFinalizacaoConversor TipoFinalizacao,
    string? MensagemFinalizacao,
    string? WhatsAppNumero,
    string? WhatsAppTextoPreDefinido,
    IReadOnlyList<ConversorPerguntaItemDto> Perguntas,
    DateTime DataCriacao,
    DateTime DataAtualizacao);
