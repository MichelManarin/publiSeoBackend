using API.Contracts;
using Application.Blog.Commands;
using Application.Blog.Contracts;
using Application.Blog.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlogController : ApiBaseController
{
    /// <summary>
    /// Lista os blogs do usuário logado.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BlogResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new ListarBlogsPorUsuarioQuery(UsuarioId.Value), cancellationToken);
        return StandardOk(result);
    }

    /// <summary>
    /// Cria um novo blog vinculado ao usuário logado (owner). Domínio pode ser vinculado depois.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BlogResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Criar([FromBody] CriarBlogRequest request, CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new CriarBlogCommand(
            UsuarioId.Value,
            request.Nome,
            request.Nicho,
            request.PalavrasChave ?? Array.Empty<string>(),
            request.UrlSlug), cancellationToken);
        if (result == null)
            return new ObjectResult(ApiResponse<object?>.Fail(StatusCodes.Status400BadRequest, "Usuário não encontrado."))
                { StatusCode = StatusCodes.Status400BadRequest };
        return StandardCreated($"/api/blog/{result.Id}", result);
    }
}

/// <summary>
/// Request para criação de blog. PalavrasChave: até 5 temas que deseja escrever.
/// </summary>
public record CriarBlogRequest(string Nome, string Nicho, IReadOnlyList<string>? PalavrasChave, string? UrlSlug = null);
