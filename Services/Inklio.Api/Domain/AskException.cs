namespace Inklio.Api.Domain;

/// <summary>
/// The exception type thrown when errors occur within the application's Ask domain.
/// </summary>
public class AskException : Exception
{
    public AskException()
    { }

    public AskException(string message)
        : base(message)
    { }

    public AskException(string message, Exception innerException)
        : base(message, innerException)
    { }
}