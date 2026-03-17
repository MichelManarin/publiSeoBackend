using API.Contracts;
using Application.BlogIntegracao.Commands;
using Application.BlogIntegracao.Contracts;
using Application.BlogIntegracao.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Integrações do blog (ex.: tag de verificação Google, Analytics) para injetar no &lt;head&gt; da página.
/// </summary>
[ApiController]
[Route("api/blog/{blogId:guid}/integrations")]
[Authorize]
public class BlogIntegracaoController : ApiBaseController
{
    /// <summary>
    /// Lista as integrações do blog (painel). Requer acesso ao blog.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BlogIntegracaoItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Listar(Guid blogId, CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new ListarIntegracoesPorBlogQuery(UsuarioId.Value, blogId), cancellationToken);
        return StandardOk(result);
    }

    /// <summary>
    /// Cria uma integração. Tipo permitido: GoogleSiteVerification (tag de verificação do Google Search Console).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BlogIntegracaoItemDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Criar(Guid blogId, [FromBody] CriarBlogIntegracaoRequest request, CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new CriarBlogIntegracaoCommand(
            UsuarioId.Value,
            blogId,
            request.Tipo,
            request.Valor ?? string.Empty,
            request.Ordem), cancellationToken);
        if (result == null)
            return StandardNotFound("Blog não encontrado ou você não tem acesso.");
        return StandardCreated($"/api/blog/{blogId}/integrations/{result.Id}", result);
    }

    /// <summary>
    /// Atualiza uma integração existente. Envie apenas os campos que deseja alterar.
    /// </summary>
    [HttpPut("{integracaoId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<BlogIntegracaoItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(Guid blogId, Guid integracaoId, [FromBody] AtualizarBlogIntegracaoRequest request, CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new AtualizarBlogIntegracaoCommand(
            UsuarioId.Value,
            blogId,
            integracaoId,
            request.Tipo,
            request.Valor,
            request.Ordem), cancellationToken);
        if (result == null)
            return StandardNotFound("Integração ou blog não encontrado.");
        return StandardOk(result);
    }

    /// <summary>
    /// Remove uma integração do blog.
    /// </summary>
    [HttpDelete("{integracaoId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(Guid blogId, Guid integracaoId, CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var ok = await Mediator.Send(new ExcluirBlogIntegracaoCommand(UsuarioId.Value, blogId, integracaoId), cancellationToken);
        if (!ok)
            return StandardNotFound("Integração ou blog não encontrado.");
        return NoContent();
    }
}

/// <summary>
/// Request para criar integração. Tipo: "GoogleSiteVerification" (tag de verificação do Google).
/// </summary>
public record CriarBlogIntegracaoRequest(string Tipo, string? Valor, int Ordem = 0);

/// <summary>
/// Request para atualizar integração. Campos opcionais: só preencha o que for alterar.
/// </summary>
public record AtualizarBlogIntegracaoRequest(string? Tipo, string? Valor, int? Ordem);
