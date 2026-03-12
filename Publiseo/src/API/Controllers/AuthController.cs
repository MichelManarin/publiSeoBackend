using API.Contracts;
using Application.Auth.Commands;
using Application.Auth.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ApiBaseController
{
    /// <summary>
    /// Login: retorna o token JWT e dados do usuário.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var result = await Mediator.Send(new LoginCommand(request.Login, request.Senha, ip), cancellationToken);
        if (result == null)
            return new ObjectResult(ApiResponse<object?>.Fail(StatusCodes.Status401Unauthorized, "Login ou senha inválidos."))
                { StatusCode = StatusCodes.Status401Unauthorized };
        return StandardOk(result);
    }

    /// <summary>
    /// Registra um novo usuário. Senha forte: mínimo 8 caracteres, maiúscula, minúscula, número e caractere especial.
    /// </summary>
    [HttpPost("registrar")]
    [ProducesResponseType(typeof(ApiResponse<RegistrarUsuarioResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registrar([FromBody] RegistrarUsuarioRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new RegistrarUsuarioCommand(
            request.Nome, request.Sobrenome, request.Email, request.Telefone, request.Login, request.Senha,
            request.Endereco, request.Cidade, request.Estado, request.CodigoPostal, request.Pais), cancellationToken);
        if (result == null)
            return new ObjectResult(ApiResponse<object?>.Fail(StatusCodes.Status400BadRequest, "Login ou e-mail já em uso."))
                { StatusCode = StatusCodes.Status400BadRequest };
        return StandardCreated($"/api/usuarios/{result.UsuarioId}", result);
    }
}
