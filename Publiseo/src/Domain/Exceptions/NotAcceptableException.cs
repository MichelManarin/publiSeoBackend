using System.Diagnostics.CodeAnalysis;

namespace Domain.Exceptions;

[ExcludeFromCodeCoverage]
public class NotAcceptableException : ApplicationException
{
    public NotAcceptableException(string message) : base("Not Acceptable", message)
    {
    }
}
