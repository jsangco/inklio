namespace Inklio.Api.Domain;

/// <summary>
/// The exception type thrown when errors occur within the application's Ask domain.
/// </summary>
public class AskDomainException : Exception
{
    public AskDomainException()
    { }

    public AskDomainException(string message)
        : base(message)
    { }

    public AskDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}