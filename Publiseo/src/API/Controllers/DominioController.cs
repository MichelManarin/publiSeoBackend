using API.Contracts;
using Application.Dominio.Commands;
using Application.Dominio.Contracts;
using Application.Dominio.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DominioController : ApiBaseController
{
    /// <summary>
    /// Verifica se um domínio está disponível para registro (via GoDaddy).
    /// </summary>
    /// <param name="dominio">Nome do domínio (ex.: exemplo.com)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("disponibilidade")]
    [ProducesResponseType(typeof(ApiResponse<DominioDisponibilidadeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerificarDisponibilidade(
        [FromQuery] string dominio,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new VerificarDisponibilidadeDominioQuery(dominio), cancellationToken);
        return StandardOk(result);
    }

    /// <summary>
    /// Sugere domínios alternativos (GoDaddy /v1/domains/suggest).
    /// </summary>
    /// <param name="query">Domínio ou palavras-chave para sugestões (obrigatório)</param>
    /// <param name="country">Código ISO do país (ex.: BR, US)</param>
    /// <param name="city">Cidade como hint de região</param>
    /// <param name="sources">Fontes: CC_TLD, EXTENSION, KEYWORD_SPIN, PREMIUM (repita o parâmetro para múltiplos)</param>
    /// <param name="tlds">TLDs a incluir (ex.: com, net). Repita o parâmetro para múltiplos</param>
    /// <param name="lengthMin">Comprimento mínimo do segundo nível</param>
    /// <param name="lengthMax">Comprimento máximo do segundo nível</param>
    /// <param name="limit">Número máximo de sugestões (1-100)</param>
    /// <param name="waitMs">Tempo máximo de espera em ms (padrão 1000)</param>
    /// <param name="shopperId">Header X-Shopper-Id (opcional)</param>
    [HttpGet("sugestao")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SugestaoDominioItemResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SugerirDominios(
        [FromQuery] string query,
        [FromQuery] string? country = null,
        [FromQuery] string? city = null,
        [FromQuery] List<string>? sources = null,
        [FromQuery] List<string>? tlds = null,
        [FromQuery] int? lengthMin = null,
        [FromQuery] int? lengthMax = null,
        [FromQuery] int? limit = null,
        [FromQuery] int? waitMs = null,
        [FromHeader(Name = "X-Shopper-Id")] string? shopperId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new SugerirDominiosQuery(
            Query: query,
            Country: country,
            City: city,
            Sources: sources,
            Tlds: tlds,
            LengthMin: lengthMin,
            LengthMax: lengthMax,
            Limit: limit,
            WaitMs: waitMs,
            ShopperId: shopperId), cancellationToken);
        return StandardOk(result);
    }

    /// <summary>
    /// Compra um domínio em nome do usuário logado (titularidade do cliente).
    /// Verifica disponibilidade, obtém termos do TLD, monta contatos (cliente + contato técnico da empresa) e executa a compra na GoDaddy.
    /// Requer autenticação. Dados do titular vêm do cadastro do usuário; contato técnico vem das configurações da plataforma.
    /// </summary>
    /// <param name="request">Domínio, período em anos, privacidade, renovação automática e IP de aceite dos termos (opcional).</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPost("compra")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<ComprarDominioResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Comprar(
        [FromBody] ComprarDominioRequest request,
        CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new ComprarDominioCommand(
            UsuarioId.Value,
            request.Dominio,
            request.Period,
            request.Privacy,
            request.RenewAuto,
            request.AgreedBy), cancellationToken);
        if (result == null)
            return new ObjectResult(ApiResponse<object?>.Fail(StatusCodes.Status400BadRequest, "Usuário não encontrado."))
                { StatusCode = StatusCodes.Status400BadRequest };
        return StandardCreated($"/api/dominio/{result.DominioId}", result);
    }
}

/// <summary>
/// Request para compra de domínio. Titularidade e contatos de admin/billing vêm do usuário; contato técnico da empresa vem das configurações.
/// </summary>
public record ComprarDominioRequest(
    string Dominio,
    int Period = 1,
    bool Privacy = false,
    bool RenewAuto = true,
    string? AgreedBy = null);
