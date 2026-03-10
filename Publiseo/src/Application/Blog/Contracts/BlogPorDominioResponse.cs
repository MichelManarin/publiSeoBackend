namespace Application.Blog.Contracts;

/// <summary>
/// Resposta pública com o ExternalId do blog encontrado pelo domínio.
/// </summary>
public record BlogPorDominioResponse(Guid ExternalId);
