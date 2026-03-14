using API.Contracts;
using Application.SearchConsole.Abstractions;
using Application.SearchConsole.Commands;
using Application.SearchConsole.Contracts;
using Application.SearchConsole.Queries;
using Infrastructure.SearchConsole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Quartz;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SearchConsoleController : ApiBaseController
{
    private const string SincronizarJobKey = "SincronizarSearchConsole";
    private const string CacheKeyPrefix = "gsc:state:";
    private static readonly TimeSpan StateExpiration = TimeSpan.FromMinutes(10);

    private readonly IScheduler _scheduler;
    private readonly IGoogleSearchConsoleOAuthService _oauthService;
    private readonly IMemoryCache _cache;
    private readonly SearchConsoleOptions _searchConsoleOptions;

    public SearchConsoleController(
        IScheduler scheduler,
        IGoogleSearchConsoleOAuthService oauthService,
        IMemoryCache cache,
        IOptions<SearchConsoleOptions> searchConsoleOptions)
    {
        _scheduler = scheduler;
        _oauthService = oauthService;
        _cache = cache;
        _searchConsoleOptions = searchConsoleOptions.Value;
    }

    /// <summary>
    /// Retorna a URL de autorização Google para o frontend redirecionar o usuário (OAuth).
    /// Use este endpoint quando o front chama a API com Authorization (ex.: SPA em outro domínio);
    /// o front faz GET com o token, recebe a URL e faz window.location.href = data.url.
    /// </summary>
    [HttpGet("connect-url")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult ConnectUrl()
    {
        if (UsuarioId == null)
            return Unauthorized();
        var state = Guid.NewGuid().ToString("N");
        _cache.Set(CacheKeyPrefix + state, UsuarioId.Value, StateExpiration);
        var url = _oauthService.BuildAuthorizationUrl(state);
        return StandardOk(new { url });
    }

    /// <summary>
    /// Gera a URL de autorização Google e redireciona o usuário para conectar a conta Search Console (OAuth).
    /// O state é armazenado em cache para associar o callback ao usuário logado.
    /// </summary>
    [HttpGet("connect")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Connect()
    {
        if (UsuarioId == null)
            return Unauthorized();
        var state = Guid.NewGuid().ToString("N");
        _cache.Set(CacheKeyPrefix + state, UsuarioId.Value, StateExpiration);
        var url = _oauthService.BuildAuthorizationUrl(state);
        return Redirect(url);
    }

    /// <summary>
    /// Callback OAuth chamado pelo Google após o usuário autorizar. Troca o code por refresh token e persiste.
    /// Redireciona para a URL de sucesso ou erro configurada em SearchConsole:OAuthFrontendSuccessUrl / OAuthFrontendErrorUrl.
    /// </summary>
    [HttpGet("callback")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Callback(
        [FromQuery] string? code,
        [FromQuery] string? state,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
            return Redirect(AppendQuery(_searchConsoleOptions.OAuthFrontendErrorUrl, "reason=missing_params"));

        if (!_cache.TryGetValue(CacheKeyPrefix + state, out Guid userId))
            return Redirect(AppendQuery(_searchConsoleOptions.OAuthFrontendErrorUrl, "reason=invalid_state"));
        _cache.Remove(CacheKeyPrefix + state);

        var success = await Mediator.Send(
            new VincularSearchConsoleOAuthCommand(userId, code, _searchConsoleOptions.OAuthRedirectUri.Trim()),
            cancellationToken);
        var redirectUrl = success
            ? _searchConsoleOptions.OAuthFrontendSuccessUrl
            : AppendQuery(_searchConsoleOptions.OAuthFrontendErrorUrl, "reason=exchange_failed");
        return Redirect(redirectUrl);
    }

    private static string AppendQuery(string url, string query)
    {
        var separator = string.IsNullOrEmpty(url) || !url.Contains('?') ? "?" : "&";
        return url + separator + query;
    }

    /// <summary>
    /// Retorna se o usuário logado possui Google Search Console conectado (OAuth) e o e-mail da conta.
    /// </summary>
    [HttpGet("status")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<SearchConsoleStatusResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Status(CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new ObterSearchConsoleStatusQuery(UsuarioId.Value), cancellationToken);
        return StandardOk(result);
    }

    /// <summary>
    /// Desvincula a conta Google Search Console do usuário logado.
    /// </summary>
    [HttpDelete("disconnect")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Disconnect(CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        await Mediator.Send(new DesvincularSearchConsoleCommand(UsuarioId.Value), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Lista métricas do Search Console por blog e período (dados já sincronizados no banco, dia a dia).
    /// Requer que o usuário tenha acesso ao blog.
    /// </summary>
    [HttpGet("blog/{blogId:guid}/metricas")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SearchConsoleMetricaResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListarMetricasPorBlog(
        Guid blogId,
        [FromQuery] DateOnly dataInicio,
        [FromQuery] DateOnly dataFim,
        CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(
            new ListarMetricasSearchConsolePorBlogQuery(UsuarioId.Value, blogId, dataInicio, dataFim),
            cancellationToken);
        if (result == null)
            return StandardNotFound("Blog não encontrado ou você não tem acesso.");
        return StandardOk(result);
    }

    /// <summary>
    /// Dispara a sincronização das métricas do Google Search Console em background (job).
    /// O job diário já roda automaticamente; use este endpoint para forçar uma sincronização manual.
    /// </summary>
    [HttpPost("sincronizar")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Sincronizar(CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        await _scheduler.TriggerJob(new JobKey(SincronizarJobKey), cancellationToken);
        return new ObjectResult(ApiResponse<object>.Ok(
            new { message = "Sincronização Search Console iniciada em background. Os dados serão atualizados em alguns instantes." },
            StatusCodes.Status202Accepted))
            { StatusCode = StatusCodes.Status202Accepted };
    }
}
