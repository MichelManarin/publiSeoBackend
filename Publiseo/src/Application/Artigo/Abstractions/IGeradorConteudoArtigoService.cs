namespace Application.Artigo.Abstractions;

/// <summary>
/// Serviço que gera conteúdo de artigo (ex.: via IA). Abstração para permitir troca de provedor (OpenAI, outro).
/// </summary>
public interface IGeradorConteudoArtigoService
{
    /// <summary>
    /// Gera o conteúdo em HTML do artigo com base no título e parâmetros.
    /// </summary>
    /// <param name="titulo">Título/tópico do artigo (usado como palavra-chave).</param>
    /// <param name="numeroPalavras">Número de palavras alvo (ex.: 500).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTML simples (apenas tags, sem body/html/css) ou null em caso de falha.</returns>
    Task<string?> GerarConteudoAsync(string titulo, int numeroPalavras, CancellationToken cancellationToken = default);
}
