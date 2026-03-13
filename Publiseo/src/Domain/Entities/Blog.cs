using System.Diagnostics.CodeAnalysis;
using Domain.Enums;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class Blog
{
    public Guid Id { get; set; }
    /// <summary>Identificador público do blog para expor conteúdo (ex.: feed/artigos) sem expor o Id interno.</summary>
    public Guid ExternalId { get; set; }
    /// <summary>Dono do blog (quem criou). Mantido para acesso direto; membros em BlogMembro.</summary>
    public Guid UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    /// <summary>Slug para URL amigável (ex.: meu-blog). Único por usuário. Usado em rotas/SEO.</summary>
    public string? UrlSlug { get; set; }
    public string Nicho { get; set; } = string.Empty;
    /// <summary>Descrição do blog (opcional).</summary>
    public string? Descricao { get; set; }
    /// <summary>Até 5 palavras-chave sobre os temas do blog.</summary>
    public List<string> PalavrasChave { get; set; } = new();
    /// <summary>Nome do autor padrão dos posts (qualquer texto). Se vazio, usar o primeiro usuário vinculado ao blog ao exibir.</summary>
    public string? AutorPadraoNome { get; set; }
    /// <summary>Objetivo principal do blog (ex.: Adsense, Saas, Informativo, Gerar Leads).</summary>
    public string? ObjetivoFinal { get; set; }
    /// <summary>Indica se existe um produto/serviço vinculado ao blog para o qual o conteúdo deve apoiar.</summary>
    public bool? PossuiProdutoVinculado { get; set; }
    /// <summary>Descrição resumida do produto/serviço vinculado ao blog (até 2000 caracteres recomendados).</summary>
    public string? DescricaoProdutoVinculado { get; set; }
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
