using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class Usuario
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Sobrenome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    /// <summary>Usado para login (pode ser o e-mail).</summary>
    public string Login { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string? Endereco { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? CodigoPostal { get; set; }
    public string Pais { get; set; } = "Brasil";
    public DateTime? UltimoLogin { get; set; }
    public string? UltimoTokenGerado { get; set; }
    public DateTime DataCriacao { get; set; }
    public string? UltimoIp { get; set; }

    public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    public ICollection<BlogMembro> BlogMembros { get; set; } = new List<BlogMembro>();
    /// <summary>Vínculo OAuth com Google Search Console (um por usuário).</summary>
    public SearchConsoleOAuth? SearchConsoleOAuth { get; set; }
}
