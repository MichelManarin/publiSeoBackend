using API.Contracts;
using Application.Conversor.Commands;
using Application.Conversor.Contracts;
using Application.Conversor.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Configuração do conversor (widget de leads) por blog. Requer autenticação.
/// </summary>
[ApiController]
[Route("api/blog/{blogId:guid}/conversor")]
[Authorize]
public class ConversorController : ApiBaseController
{
    /// <summary>
    /// Obtém a configuração do conversor do blog. Retorna 404 se não existir.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<ConversorConfigResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Obter(Guid blogId, CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new ObterConversorPorBlogQuery(UsuarioId.Value, blogId), cancellationToken);
        if (result == null)
            return StandardNotFound("Conversor não encontrado ou você não tem acesso ao blog.");
        return StandardOk(result);
    }

    /// <summary>
    /// Cria ou atualiza a configuração do conversor do blog.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<ConversorConfigResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Salvar(Guid blogId, [FromBody] SalvarConversorRequest request, CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new SalvarConversorCommand(UsuarioId.Value, blogId, request), cancellationToken);
        if (result == null)
            return StandardNotFound("Blog não encontrado ou você não tem acesso.");
        return StandardOk(result);
    }
}
