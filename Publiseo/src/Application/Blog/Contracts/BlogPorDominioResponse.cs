using Application.BlogIntegracao.Contracts;

namespace Application.Blog.Contracts;

/// <summary>
/// Resposta pública com o ExternalId, nome, nicho e integrações do blog encontrado pelo domínio.
/// </summary>
public record BlogPorDominioResponse(
    Guid ExternalId,
    string Nome,
    string Nicho,
    string? Descricao,
    IReadOnlyList<IntegracaoPublicaItemDto> Integracoes);
