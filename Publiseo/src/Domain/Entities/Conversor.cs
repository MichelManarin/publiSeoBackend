using System.Diagnostics.CodeAnalysis;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Configuração do conversor (widget de captura de leads) por blog. 1:1 com Blog.
/// </summary>
[ExcludeFromCodeCoverage]
public class Conversor
{
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    /// <summary>Se true, o front exibe o widget no blog.</summary>
    public bool Ativo { get; set; }
    /// <summary>Texto do botão que abre o conversor (ex.: "Fale conosco").</summary>
    public string? TextoBotaoInicial { get; set; }
    /// <summary>Mensagem ou WhatsApp ao finalizar.</summary>
    public TipoFinalizacaoConversor TipoFinalizacao { get; set; }
    /// <summary>Quando TipoFinalizacao = Mensagem.</summary>
    public string? MensagemFinalizacao { get; set; }
    /// <summary>Número para link WhatsApp (ex.: 5511999999999). Quando TipoFinalizacao = WhatsApp.</summary>
    public string? WhatsAppNumero { get; set; }
    /// <summary>Texto pré-preenchido no link wa.me (opcional).</summary>
    public string? WhatsAppTextoPreDefinido { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }

    public Blog Blog { get; set; } = null!;
    public ICollection<ConversorPergunta> Perguntas { get; set; } = new List<ConversorPergunta>();
    public ICollection<ConversorLead> Leads { get; set; } = new List<ConversorLead>();
}
