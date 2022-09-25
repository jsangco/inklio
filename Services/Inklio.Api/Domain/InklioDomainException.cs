namespace Inklio.Api.Domain;

/// <summary>
/// The exception type thrown when errors occur within the application's Inklio domain.
/// </summary>
public class InklioDomainException : Exception
{
    public int RecommendedStatusCode { get; }

    public InklioDomainException(int recommendedStatusCode)
    {
        this.RecommendedStatusCode = recommendedStatusCode;
    }

    public InklioDomainException(int recommendedStatusCode, string message)
        : base(message)
    {
        this.RecommendedStatusCode = recommendedStatusCode;
    }

    public InklioDomainException(int recommendedStatusCode, string message, Exception innerException)
        : base(message, innerException)
    {
        this.RecommendedStatusCode = recommendedStatusCode;
    }
}