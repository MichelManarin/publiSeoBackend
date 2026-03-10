using System.Diagnostics.CodeAnalysis;
using Domain.Enums;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class Artigo
{
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? MetaDescription { get; set; }
    public string Conteudo { get; set; } = string.Empty;
    public TipoRascunho TipoRascunho { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public Guid UltimoUsuarioId { get; set; }

    public Blog Blog { get; set; } = null!;
    public Usuario UltimoUsuario { get; set; } = null!;
}
