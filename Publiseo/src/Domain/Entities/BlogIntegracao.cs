using Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

/// <summary>
/// Integração vinculada ao blog (ex.: tag de verificação Google).
/// O front injeta o Valor no &lt;head&gt; da página do blog.
/// </summary>
[ExcludeFromCodeCoverage]
public class BlogIntegracao
{
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    public BlogIntegracaoTipo Tipo { get; set; }
    /// <summary>Conteúdo a injetar no head (ex.: tag meta completa, script).</summary>
    public string Valor { get; set; } = string.Empty;
    /// <summary>Ordem de exibição no head (menor = primeiro).</summary>
    public int Ordem { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }

    public Blog Blog { get; set; } = null!;
}
