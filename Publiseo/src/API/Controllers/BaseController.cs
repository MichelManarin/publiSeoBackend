using System.Security.Claims;
using API.Contracts;
using Application.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers;

public abstract class ApiBaseController : ControllerBase
{
    private IApplicationMediator? _mediator;

    /// <summary>
    /// Mediator desacoplado: use Send(command) ou Send(query) para disparar handlers da Application.
    /// </summary>
    protected IApplicationMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IApplicationMediator>();

    /// <summary>
    /// Id do usuário logado (do JWT). Null se não autenticado.
    /// </summary>
    protected Guid? UsuarioId
    {
        get
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            return Guid.TryParse(sub, out var id) ? id : null;
        }
    }

    protected ICollection<string> Erros = new List<string>();

    protected bool HasErros() => Erros.Count != 0;

    protected ActionResult StandardOk<T>(T data) =>
        new ObjectResult(ApiResponse<T>.Ok(data, StatusCodes.Status200OK)) { StatusCode = StatusCodes.Status200OK };

    protected ActionResult StandardCreated<T>(string location, T data) =>
        new ObjectResult(ApiResponse<T>.Ok(data, StatusCodes.Status201Created)) { StatusCode = StatusCodes.Status201Created };

    protected ActionResult StandardNotFound(string? message = null) =>
        new ObjectResult(ApiResponse<object?>.Fail(StatusCodes.Status404NotFound, message ?? "Recurso não encontrado"))
            { StatusCode = StatusCodes.Status404NotFound };

    protected ActionResult CustomResponse(object? result = null)
    {
        if (HasErros())
            return new BadRequestObjectResult(CreateValidationProblemDetails());
        if (result == null)
            return new NoContentResult();
        return new ObjectResult(result);
    }

    protected ActionResult CustomResponse(ValidationException validationResult)
    {
        foreach (var error in validationResult.Errors)
            AddError($"{error.PropertyName} - {error.ErrorMessage}");
        return CustomResponse();
    }

    protected ActionResult CustomResponse<T>(Domain.Helpers.BaseResponse<T> response) where T : class
    {
        return response.Success ? new ObjectResult(response) : CustomResponse(response.ValidationResult!);
    }

    protected void AddError(string errorMessage) => Erros.Add(errorMessage);

    private ValidationProblemDetails CreateValidationProblemDetails()
    {
        var modelState = new ModelStateDictionary();
        foreach (var erro in Erros)
            modelState.AddModelError("Messages", erro);
        return new ValidationProblemDetails(modelState);
    }
}
