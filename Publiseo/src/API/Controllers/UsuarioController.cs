using API.Contracts;
using Application.Auth.Commands;
using Application.Auth.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuarioController : ApiBaseController
{
    /// <summary>
    /// Edita o perfil do usuário autenticado. Apenas o próprio usuário pode editar (identificado pelo token).
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<EditarUsuarioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Editar([FromBody] EditarUsuarioRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new EditarUsuarioCommand(
            request.Nome,
            request.Sobrenome,
            request.Email,
            request.Telefone,
            request.Endereco,
            request.Cidade,
            request.Estado,
            request.CodigoPostal,
            request.Pais), cancellationToken);
        if (result == null)
            return new ObjectResult(ApiResponse<object?>.Fail(StatusCodes.Status400BadRequest, "Usuário não encontrado ou e-mail já em uso por outra conta."))
                { StatusCode = StatusCodes.Status400BadRequest };
        return StandardOk(result);
    }
}
