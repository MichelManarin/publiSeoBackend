using System.Diagnostics.CodeAnalysis;
using Domain.Enums;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class Blog
{
    public Guid Id { get; set; }
    /// <summary>Dono do blog (quem criou). Mantido para acesso direto; membros em BlogMembro.</summary>
    public Guid UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    /// <summary>Domínio (pode ser vinculado depois).</summary>
    public string? UrlSlug { get; set; }
    public string Nicho { get; set; } = string.Empty;
    /// <summary>Até 5 palavras-chave sobre os temas do blog.</summary>
    public List<string> PalavrasChave { get; set; } = new();
    public DateTime DataCriacao { get; set; }

    public Usuario Usuario { get; set; } = null!;
    public ICollection<BlogMembro> Membros { get; set; } = new List<BlogMembro>();
    /// <summary>Domínios configurados para este blog (um blog pode ter vários).</summary>
    public ICollection<BlogDominio> Dominios { get; set; } = new List<BlogDominio>();
}

[ExcludeFromCodeCoverage]
public class BlogMembro
{
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    public Guid UsuarioId { get; set; }
    public PapelBlog Papel { get; set; }
    public DateTime DataVinculo { get; set; }

    public Blog Blog { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
}
