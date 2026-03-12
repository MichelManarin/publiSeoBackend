using API.Contracts;
using Application.Artigo.Commands;
using Application.Artigo.Contracts;
using Application.Artigo.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ArtigoController : ApiBaseController
{
    private const string ProcessarPendentesJobKey = "ProcessarArtigosPendentes";
    private readonly IScheduler _scheduler;

    public ArtigoController(IScheduler scheduler)
    {
        _scheduler = scheduler;
    }
    [HttpGet("blog/{blogId}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ArtigoResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Listar(Guid blogId, CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new ListarArtigosQuery(blogId), cancellationToken);
        return StandardOk(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ArtigoResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Criar([FromBody] CriarArtigoRequest request, CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new CriarArtigoCommand(
            request.BlogId,
            UsuarioId.Value,
            request.Titulo,
            request.MetaDescription,
            request.Conteudo,
            request.TipoRascunho), cancellationToken);
        if (result == null)
            return new ObjectResult(ApiResponse<object?>.Fail(StatusCodes.Status400BadRequest, "Blog ou usuário não encontrado."))
                { StatusCode = StatusCodes.Status400BadRequest };
        return StandardCreated($"/api/artigo/{result.Id}", result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ArtigoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Editar(Guid id, [FromBody] EditarArtigoRequest request, CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var result = await Mediator.Send(new EditarArtigoCommand(
            id,
            UsuarioId.Value,
            request.Titulo,
            request.MetaDescription,
            request.Conteudo,
            request.TipoRascunho), cancellationToken);
        if (result == null)
            return StandardNotFound("Artigo ou usuário não encontrado.");
        return StandardOk(result);
    }

    /// <summary>
    /// Exclui um artigo. Apenas usuários vinculados ao blog (dono ou membro) podem excluir.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(Guid id, CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        var excluido = await Mediator.Send(new ExcluirArtigoCommand(id, UsuarioId.Value), cancellationToken);
        if (!excluido)
            return StandardNotFound("Artigo não encontrado ou você não tem acesso.");
        return NoContent();
    }

    /// <summary>
    /// Dispara o job de processamento dos artigos com geração por IA pendente e retorna imediatamente (202). O processamento roda em background.
    /// </summary>
    [HttpPost("processar-pendentes")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ProcessarPendentes(CancellationToken cancellationToken)
    {
        if (UsuarioId == null)
            return Unauthorized();
        await _scheduler.TriggerJob(new JobKey(ProcessarPendentesJobKey), cancellationToken);
        return new ObjectResult(ApiResponse<object>.Ok(
            new { message = "Processamento iniciado em background. Atualize a lista de artigos em alguns instantes." },
            StatusCodes.Status202Accepted))
            { StatusCode = StatusCodes.Status202Accepted };
    }
}

public record CriarArtigoRequest(
    Guid BlogId,
    string Titulo,
    string? MetaDescription,
    string Conteudo,
    Domain.Enums.TipoRascunho TipoRascunho);

public record EditarArtigoRequest(
    string Titulo,
    string? MetaDescription,
    string Conteudo,
    Domain.Enums.TipoRascunho TipoRascunho);
