namespace Inklio.Api.Domain;

/// <summary>
/// The exception type thrown when errors occur within the application's Inklio domain.
/// </summary>
public class InklioDomainException : Exception
{
    /// <summary>
    /// Gets a recommend status code to use. Typically an HTTP status code.
    /// </summary>
    public int RecommendedStatusCode { get; }

    /// <summary>
    /// Gets an optional collection of errors.
    /// </summary>
    public IReadOnlyDictionary<string, string[]>? Errors { get; }

    /// <summary>
    /// Initializes an instance of a <see cref="InklioDomainException"/> object.
    /// </summary>
    /// <param name="recommendedStatusCode">A recommend status code to use. Typically an HTTP status code.</param>
    public InklioDomainException(int recommendedStatusCode)
    {
        this.RecommendedStatusCode = recommendedStatusCode;
    }

    /// <summary>
    /// Initializes an instance of a <see cref="InklioDomainException"/> object.
    /// </summary>
    /// <param name="recommendedStatusCode">A recommend status code to use. Typically an HTTP status code.</param>
    /// <param name="message">A message that user will see.</param>
    public InklioDomainException(int recommendedStatusCode, string message)
        : base(message)
    {
        this.RecommendedStatusCode = recommendedStatusCode;
    }

    /// <summary>
    /// Initializes an instance of a <see cref="InklioDomainException"/> object.
    /// </summary>
    /// <param name="recommendedStatusCode">A recommend status code to use. Typically an HTTP status code.</param>
    /// <param name="message">A message that user will see.</param>
    /// <param name="innerException">An inner exception</param>
    public InklioDomainException(int recommendedStatusCode, string message, Exception innerException)
        : base(message, innerException)
    {
        this.RecommendedStatusCode = recommendedStatusCode;
    }

    /// <summary>
    /// Initializes an instance of a <see cref="InklioDomainException"/> object.
    /// </summary>
    /// <param name="recommendedStatusCode">A recommend status code to use. Typically an HTTP status code.</param>
    /// <param name="message">A message that user will see.</param>
    /// <param name="errors">An optional collection of errors.</param>
    /// <param name="innerException">An inner exception</param>
    public InklioDomainException(int recommendedStatusCode, IDictionary<string, string[]> errors)
        : base()
    {
        this.RecommendedStatusCode = recommendedStatusCode;
        this.Errors = errors.ToDictionary(i => i.Key, i => i.Value);
    }

    /// <summary>
    /// Initializes an instance of a <see cref="InklioDomainException"/> object.
    /// </summary>
    /// <param name="recommendedStatusCode">A recommend status code to use. Typically an HTTP status code.</param>
    /// <param name="message">A message that user will see.</param>
    /// <param name="errors">An optional collection of errors.</param>
    /// <param name="innerException">An inner exception</param>
    public InklioDomainException(int recommendedStatusCode, string message, IDictionary<string, string[]> errors)
        : base(message)
    {
        this.RecommendedStatusCode = recommendedStatusCode;
        this.Errors = errors.ToDictionary(i => i.Key, i => i.Value);
    }

    /// <summary>
    /// Initializes an instance of a <see cref="InklioDomainException"/> object.
    /// </summary>
    /// <param name="recommendedStatusCode">A recommend status code to use. Typically an HTTP status code.</param>
    /// <param name="message">A message that user will see.</param>
    /// <param name="errors">An optional collection of errors.</param>
    /// <param name="innerException">An inner exception</param>
    public InklioDomainException(int recommendedStatusCode, string message, IDictionary<string, string[]> errors, Exception innerException)
        : base(message, innerException)
    {
        this.RecommendedStatusCode = recommendedStatusCode;
        this.Errors = errors.ToDictionary(i => i.Key, i => i.Value);
    }
}