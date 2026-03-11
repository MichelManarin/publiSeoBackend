namespace Application.Blog.Contracts;

/// <summary>
/// Resposta pública com o ExternalId, nome e nicho do blog encontrado pelo domínio.
/// </summary>
public record BlogPorDominioResponse(Guid ExternalId, string Nome, string Nicho);
