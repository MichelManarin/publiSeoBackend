using System.Diagnostics.CodeAnalysis;

namespace Domain.Exceptions;

[ExcludeFromCodeCoverage]
public class NotFoundException : ApplicationException
{
    public NotFoundException(string message) : base("Not Found", message)
    {
    }
}
