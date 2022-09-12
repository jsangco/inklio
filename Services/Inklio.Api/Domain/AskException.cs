namespace Inklio.Api.Domain;

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