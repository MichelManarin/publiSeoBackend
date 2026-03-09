namespace Application.Dominio.Contracts;

/// <summary>
/// Opções para a API de sugestão de domínios (GoDaddy).
/// </summary>
public record SugestaoDominioOptions
{
    /// <summary>Nome do domínio ou palavras-chave para sugestões.</summary>
    public string Query { get; init; } = string.Empty;
    /// <summary>Código ISO do país (ex.: BR, US).</summary>
    public string? Country { get; init; }
    /// <summary>Nome da cidade como hint de região.</summary>
    public string? City { get; init; }
    /// <summary>Fontes: CC_TLD, EXTENSION, KEYWORD_SPIN, PREMIUM.</summary>
    public IReadOnlyList<string>? Sources { get; init; }
    /// <summary>TLDs a incluir (ex.: com, com.br).</summary>
    public IReadOnlyList<string>? Tlds { get; init; }
    /// <summary>Comprimento mínimo do segundo nível.</summary>
    public int? LengthMin { get; init; }
    /// <summary>Comprimento máximo do segundo nível.</summary>
    public int? LengthMax { get; init; }
    /// <summary>Número máximo de sugestões.</summary>
    public int? Limit { get; init; }
    /// <summary>Tempo máximo de espera em ms (padrão 1000).</summary>
    public int? WaitMs { get; init; }
}
