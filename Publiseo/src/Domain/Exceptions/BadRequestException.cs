using System.Diagnostics.CodeAnalysis;

namespace Domain.Exceptions;

[ExcludeFromCodeCoverage]
public class BadRequestException : ApplicationException
{
    public BadRequestException(string message) : base("Bad Request", message)
    {
    }
}
