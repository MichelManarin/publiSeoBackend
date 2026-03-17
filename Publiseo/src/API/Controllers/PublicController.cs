using API.Contracts;
using Application.Artigo.Contracts;
using Application.Artigo.Queries;
using Application.Blog.Contracts;
using Application.Blog.Queries;
using Application.BlogIntegracao.Contracts;
using Application.BlogIntegracao.Queries;
using Application.Conversor.Commands;
using Application.Conversor.Contracts;
using Application.Conversor.Queries;
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

    /// <summary>
    /// Lista integrações do blog (tags/snippets para &lt;head&gt;) por ExternalId. Para o front injetar no &lt;head&gt; da página.
    /// </summary>
    [HttpGet("blog/{externalId:guid}/integrations")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<IntegracaoPublicaItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarIntegracoesPorBlog(Guid externalId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ObterIntegracoesPublicasPorBlogQuery(externalId), cancellationToken);
        return StandardOk(result);
    }

    /// <summary>
    /// Obtém a configuração pública do conversor do blog (por ExternalId). Se ativo, retorna perguntas e tipo de finalização para o front renderizar o widget.
    /// </summary>
    [HttpGet("blog/{externalId:guid}/conversor")]
    [ProducesResponseType(typeof(ApiResponse<ConversorPublicoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterConversor(Guid externalId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ObterConversorPublicoPorBlogQuery(externalId), cancellationToken);
        if (result == null)
            return StandardNotFound("Conversor não encontrado ou inativo para este blog.");
        return StandardOk(result);
    }

    /// <summary>
    /// Registra um lead do conversor (quando o usuário completa o fluxo no widget).
    /// </summary>
    [HttpPost("conversor/lead")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegistrarLead([FromBody] RegistrarConversorLeadRequest request, CancellationToken cancellationToken)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var ok = await Mediator.Send(new RegistrarConversorLeadCommand(
            request.BlogExternalId,
            request.NomeCompleto,
            request.Telefone,
            request.Respostas ?? Array.Empty<string>(),
            request.ArtigoId,
            ip), cancellationToken);
        if (!ok)
            return StandardNotFound("Conversor não encontrado ou inativo para este blog.");
        return NoContent();
    }
}
