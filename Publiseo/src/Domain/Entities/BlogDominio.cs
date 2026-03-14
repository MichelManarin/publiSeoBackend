using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

/// <summary>
/// Domínio configurado para um blog. Um blog pode ter vários domínios (ex.: meusite.com, www.meusite.com).
/// Por enquanto armazena apenas o nome; pode ser estendido para vincular artigo etc.
/// </summary>
[ExcludeFromCodeCoverage]
public class BlogDominio
{
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    /// <summary>Nome do domínio (ex.: meusite.com).</summary>
    public string NomeDominio { get; set; } = string.Empty;

    public Blog Blog { get; set; } = null!;
    public ICollection<SearchConsoleMetrica> MetricasSearchConsole { get; set; } = new List<SearchConsoleMetrica>();
}
