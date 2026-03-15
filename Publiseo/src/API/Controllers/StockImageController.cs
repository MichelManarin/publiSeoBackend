using API.Contracts;
using Application.StockImage.Abstractions;
using Application.StockImage.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StockImageController : ApiBaseController
{
    private readonly IStockImageSearchAdapter _stockImageAdapter;

    public StockImageController(IStockImageSearchAdapter stockImageAdapter)
    {
        _stockImageAdapter = stockImageAdapter;
    }

    /// <summary>
    /// Busca imagens de estoque por termo (provedor configurado no backend: ex. Unsplash).
    /// Retorna URLs com atribuição quando aplicável. Use para capa do artigo ou inserir no texto.
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<StockImageSearchResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> Search(
        [FromQuery] string? q,
        [FromQuery] int page = 1,
        [FromQuery] int per_page = 10,
        CancellationToken cancellationToken = default)
    {
        if (UsuarioId == null)
            return Unauthorized();
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(ApiResponse<object>.Fail(400, "O parâmetro 'q' (termo de busca) é obrigatório."));

        var result = await _stockImageAdapter.SearchAsync(q.Trim(), page, per_page, cancellationToken);
        if (result == null)
            return new ObjectResult(ApiResponse<object>.Fail(502, "Não foi possível buscar imagens. Tente novamente."))
                { StatusCode = StatusCodes.Status502BadGateway };

        return StandardOk(result);
    }
}
