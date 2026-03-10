using API.Contracts;
using Application.Artigo.Contracts;
using Application.Artigo.Queries;
using Application.Blog.Contracts;
using Application.Blog.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Endpoints públicos (sem autenticação) para expor conteúdo do blog por ExternalId ou domínio.
/// </summary>
[ApiController]
[Route("api/public")]
[AllowAnonymous]
public class PublicController : ApiBaseController
{
    /// <summary>
    /// Obtém o ExternalId do blog pelo domínio de origem. Verifica se existe um blog com aquele domínio em blog_dominios.
    /// </summary>
    [HttpGet("blog/por-dominio")]
    [ProducesResponseType(typeof(ApiResponse<BlogPorDominioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterBlogPorDominio([FromQuery] string dominio, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dominio))
            return new ObjectResult(ApiResponse<object?>.Fail(StatusCodes.Status400BadRequest, "Domínio não informado."))
                { StatusCode = StatusCodes.Status400BadRequest };
        var result = await Mediator.Send(new ObterBlogPorDominioQuery(dominio.Trim()), cancellationToken);
        if (result == null)
            return StandardNotFound("Nenhum blog encontrado para este domínio.");
        return StandardOk(result);
    }

    /// <summary>
    /// Lista os artigos de um blog pelo ExternalId do blog. Retorna nome, descrição, data de publicação, conteúdo e autor.
    /// </summary>
    [HttpGet("blog/{externalId:guid}/artigos")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ArtigoPublicoResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListarArtigosPorBlog(Guid externalId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ListarArtigosPublicosPorBlogQuery(externalId), cancellationToken);
        if (result == null)
            return StandardNotFound("Blog não encontrado.");
        return StandardOk(result);
    }
}
