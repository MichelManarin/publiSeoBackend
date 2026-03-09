using System.Diagnostics.CodeAnalysis;
using ApplicationException = Domain.Exceptions.ApplicationException;

namespace Application.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class ValidationException : ApplicationException
{
    public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary)
        : base("Validation Failure", "One or more validation errors occurred")
        => ErrorsDictionary = errorsDictionary;

    public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }
}
