using API.Contracts;
using Application.Exceptions;
using Domain.Exceptions;
using System.Text.Json;
using ApplicationException = Domain.Exceptions.ApplicationException;

namespace API.Middleware;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var response = ApiResponse<object?>.Fail(
            statusCode,
            exception.Message,
            GetErrors(exception));
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status400BadRequest,
            NotAcceptableException => StatusCodes.Status406NotAcceptable,
            _ => StatusCodes.Status500InternalServerError
        };

    private static IReadOnlyDictionary<string, string[]>? GetErrors(Exception exception)
    {
        return exception is ValidationException validationException ? validationException.ErrorsDictionary : null;
    }
}
