using API.Contracts;
using Application.KeywordResearch.Contracts;
using Application.KeywordResearch.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Pesquisa de keywords (volume de busca, concorrência, CPC, etc.) via provedor configurável (ex.: Data for SEO).
/// Dados apenas exibidos; nada é persistido no banco.
/// </summary>
[ApiController]
[Route("api/keyword-research")]
[Authorize]
public class KeywordResearchController : ApiBaseController
{
    /// <summary>
    /// Pesquisa keywords relacionadas com métricas (volume mensal, concorrência, CPC, tendência, etc.). Resposta paginada.
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<KeywordSearchPaginatedResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> Search(
        [FromQuery] string? keyword,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? locationCode = null,
        [FromQuery] string? languageCode = null,
        CancellationToken cancellationToken = default)
    {
        if (UsuarioId == null)
            return Unauthorized();
        if (string.IsNullOrWhiteSpace(keyword))
            return new ObjectResult(ApiResponse<object?>.Fail(StatusCodes.Status400BadRequest, "O parâmetro 'keyword' é obrigatório."))
                { StatusCode = StatusCodes.Status400BadRequest };

        var result = await Mediator.Send(new PesquisarKeywordsQuery(
            keyword.Trim(),
            page,
            pageSize,
            locationCode,
            string.IsNullOrWhiteSpace(languageCode) ? null : languageCode.Trim()), cancellationToken);

        if (result == null)
            return new ObjectResult(ApiResponse<object?>.Fail(StatusCodes.Status502BadGateway, "Não foi possível obter dados de keywords. Verifique a configuração do provedor (ex.: Data for SEO)."))
                { StatusCode = StatusCodes.Status502BadGateway };

        return StandardOk(result);
    }
}
