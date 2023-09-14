using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

/// <summary>
/// The default logger to use.
/// </summary>
public static class ClientLogger
{
    /// <summary>
    /// The default logger to use.
    /// </summary>
    public static ILoggerFactory DefaultLoggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });
}
