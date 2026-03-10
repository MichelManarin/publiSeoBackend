using API.Contracts;
using Application.Dashboard.Contracts;
using Application.Dashboard.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ApiBaseController
{
    /// <summary>
    /// Retorna as estatísticas do dashboard: número de blogs e número de artigos dos blogs que o usuário tem acesso.
    /// Resposta no padrão da API: ApiResponse com success, data (blogs, artigos), statusCode, message e errors.
    /// </summary>
    [HttpGet("estatisticas")]
    [ProducesResponseType(typeof(ApiResponse<DashboardEstatisticasResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Estatisticas(CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new DashboardEstatisticasQuery(UsuarioId.Value), cancellationToken);
        return StandardOk(result);
    }
}
