namespace Application.SearchConsole.Abstractions;

/// <summary>
/// Cliente para obter métricas do Google Search Console (impressões, cliques, CTR, posição média).
/// Abstração para permitir troca de implementação ou testes.
/// </summary>
public interface ISearchConsoleClient
{
    /// <summary>
    /// Obtém métricas agregadas para um site em um único dia.
    /// </summary>
    /// <param name="siteUrl">URL do site no GSC (ex.: https://www.exemplo.com/). Deve estar verificado.</param>
    /// <param name="data">Data dos dados (YYYY-MM-DD).</param>
    /// <param name="tipoBusca">Tipo de busca: web, image, video, etc.</param>
    /// <param name="refreshToken">Se informado, usa OAuth do usuário; caso contrário usa Service Account (se configurada).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Métricas do dia ou null se não houver dados ou falha.</returns>
    Task<SearchConsoleMetricasDto?> ObterMetricasAgregadasAsync(
        string siteUrl,
        DateOnly data,
        string tipoBusca = "web",
        string? refreshToken = null,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Serviço do fluxo OAuth para vincular conta Google Search Console ao usuário.
/// </summary>
public interface IGoogleSearchConsoleOAuthService
{
    /// <summary>Gera a URL de autorização Google (redirect do usuário).</summary>
    string BuildAuthorizationUrl(string state);
    /// <summary>Troca o code pelo refresh token e opcionalmente obtém o e-mail da conta Google.</summary>
    Task<(string RefreshToken, string? Email)> ExchangeCodeForTokenAsync(string code, string redirectUri, CancellationToken cancellationToken = default);
}

/// <summary>
/// DTO com métricas retornadas pela API Search Analytics (agregado por data).</summary>
public record SearchConsoleMetricasDto(
    long Impressoes,
    long Cliques,
    double Ctr,
    double PosicaoMedia);
