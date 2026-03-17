namespace Application.BlogIntegracao.Contracts;

/// <summary>
/// Item de integração do blog para listagem/edição no painel.
/// </summary>
public record BlogIntegracaoItemDto(
    Guid Id,
    string Tipo,
    string Valor,
    int Ordem,
    DateTime DataCriacao,
    DateTime DataAtualizacao);
