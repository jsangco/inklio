namespace Inklio.Api.Client;

/// <summary>
/// The exception type thrown when errors occur within in the Inklio client library.
/// </summary>
public class InklioClientException : Exception
{
    /// <summary>
    /// Gets a recommend status code to use. Typically an HTTP status code.
    /// </summary>
    public int? StatusCode { get; }

    /// <summary>
    /// Gets an optional collection of errors.
    /// </summary>
    public IReadOnlyDictionary<string, string[]>? Errors { get; }

    /// <summary>
    /// Initializes an instance of a <see cref="InklioClientException"/> object.
    /// </summary>
    /// <param name="statusCode">A status code to include in the error. Typically an HTTP status code.</param>
    public InklioClientException(int statusCode)
    {
        this.StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes an instance of a <see cref="InklioClientException"/> object.
    /// </summary>
    /// <param name="statusCode">A status code to include in the error. Typically an HTTP status code.</param>
    /// <param name="message">A message that user will see.</param>
    public InklioClientException(int statusCode, string message)
        : base(message)
    {
        this.StatusCode = statusCode;
    }


    /// <summary>
    /// Initializes an instance of a <see cref="InklioClientException"/> object.
    /// </summary>
    /// <param name="message">A message that user will see.</param>
    public InklioClientException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes an instance of a <see cref="InklioClientException"/> object.
    /// </summary>
    /// <param name="message">A message that user will see.</param>
    /// <param name="innerException">An inner exception</param>
    public InklioClientException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes an instance of a <see cref="InklioClientException"/> object.
    /// </summary>
    /// <param name="statusCode">A status code to include in the error. Typically an HTTP status code.</param>
    /// <param name="message">A message that user will see.</param>
    /// <param name="innerException">An inner exception</param>
    public InklioClientException(int statusCode, string message, Exception innerException)
        : base(message, innerException)
    {
        this.StatusCode = statusCode;
    }
}