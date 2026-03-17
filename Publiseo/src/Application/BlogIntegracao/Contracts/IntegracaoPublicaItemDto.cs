namespace Application.BlogIntegracao.Contracts;

/// <summary>
/// Item de integração para o front injetar no &lt;head&gt; da página do blog (endpoint público).
/// </summary>
public record IntegracaoPublicaItemDto(string Tipo, string Valor);
