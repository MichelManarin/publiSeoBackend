using API.Contracts;
using Application.BlogDominio.Commands;
using Application.BlogDominio.Contracts;
using Application.BlogDominio.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlogDominioController : ApiBaseController
{
    /// <summary>
    /// Lista os domínios vinculados a um blog. Apenas blogs que o usuário tem acesso (dono ou membro).
    /// </summary>
    [HttpGet("blog/{blogId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BlogDominioResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListarPorBlog(Guid blogId, CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new ListarDominiosPorBlogQuery(UsuarioId.Value, blogId), cancellationToken);
        if (result == null)
            return StandardNotFound("Blog não encontrado ou você não tem acesso.");
        return StandardOk(result);
    }

    /// <summary>
    /// Cria um domínio vinculado a um blog. Valida se o domínio já não está em uso por outro usuário ou em outro blog seu.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BlogDominioResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Criar([FromBody] CriarBlogDominioRequest request, CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new CriarBlogDominioCommand(UsuarioId.Value, request.BlogId, request.NomeDominio), cancellationToken);
        if (result == null)
            return new ObjectResult(ApiResponse<object?>.Fail(StatusCodes.Status400BadRequest, "Blog não encontrado ou você não tem acesso."))
                { StatusCode = StatusCodes.Status400BadRequest };
        return StandardCreated($"/api/blogdominio/{result.Id}", result);
    }

    /// <summary>
    /// Remove um domínio do blog. Apenas se o domínio pertencer a um blog que o usuário tem acesso.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(Guid id, CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var removido = await Mediator.Send(new RemoverBlogDominioCommand(UsuarioId.Value, id), cancellationToken);
        if (!removido)
            return StandardNotFound("Domínio não encontrado ou você não tem acesso.");
        return NoContent();
    }
}

/// <summary>
/// Request para vincular um domínio a um blog.
/// </summary>
public record CriarBlogDominioRequest(Guid BlogId, string NomeDominio);
