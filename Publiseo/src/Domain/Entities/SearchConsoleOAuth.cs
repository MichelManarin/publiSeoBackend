using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

/// <summary>
/// Vínculo OAuth do usuário com o Google Search Console (refresh token).
/// Um usuário conecta sua conta Google uma vez; usamos o refresh token para buscar métricas dos sites dele.
/// </summary>
[ExcludeFromCodeCoverage]
public class SearchConsoleOAuth
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    /// <summary>Refresh token do OAuth (guardar com cuidado; considerar criptografia em repouso).</summary>
    public string RefreshToken { get; set; } = string.Empty;
    /// <summary>E-mail da conta Google conectada (opcional, para exibir ao usuário).</summary>
    public string? EmailGoogle { get; set; }
    public DateTime DataVinculo { get; set; }

    public Usuario Usuario { get; set; } = null!;
}
