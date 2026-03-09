using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Helpers;

[ExcludeFromCodeCoverage]
public class BaseResponse<T> where T : notnull
{
    public T Data { get; }

    public BaseResponse(T data) => Data = data;

    public ValidationResult? ValidationResult { get; }

    public BaseResponse(ValidationResult validationResult)
    {
        Data = default!;
        ValidationResult = validationResult;
    }

    public BaseResponse(T result, ValidationResult validationResult) : this(result) => ValidationResult = validationResult;

    public bool Success => ValidationResult?.IsValid ?? true;
}
