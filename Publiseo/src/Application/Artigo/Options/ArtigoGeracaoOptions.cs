namespace Application.Artigo.Options;

/// <summary>
/// Opções para geração de artigos por IA (número de palavras padrão e máximo de tentativas).
/// </summary>
public class ArtigoGeracaoOptions
{
    public const string SectionName = "ArtigoGeracao";

    /// <summary>Número de palavras alvo para o artigo gerado por IA (default 500).</summary>
    public int NumeroPalavrasDefault { get; set; } = 500;
    /// <summary>Máximo de tentativas de geração por artigo antes de marcar como Falha.</summary>
    public int MaxTentativas { get; set; } = 2;
}
