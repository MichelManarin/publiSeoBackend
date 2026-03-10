using API.Contracts;
using Application.BlogDominio.Commands;
using Application.BlogDominio.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlogDominioController : ApiBaseController
{
    /// <summary>
    /// Cria um domínio vinculado a um blog. Por enquanto informa apenas o nome do domínio; um blog pode ter vários domínios.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BlogDominioResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Criar([FromBody] CriarBlogDominioRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CriarBlogDominioCommand(request.BlogId, request.NomeDominio), cancellationToken);
        if (result == null)
            return new ObjectResult(ApiResponse<object?>.Fail(StatusCodes.Status400BadRequest, "Blog não encontrado."))
                { StatusCode = StatusCodes.Status400BadRequest };
        return StandardCreated($"/api/blogdominio/{result.Id}", result);
    }
}

/// <summary>
/// Request para vincular um domínio a um blog.
/// </summary>
public record CriarBlogDominioRequest(Guid BlogId, string NomeDominio);
