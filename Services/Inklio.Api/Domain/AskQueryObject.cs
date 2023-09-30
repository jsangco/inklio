namespace Inklio.Api.Domain;

/// <summary>
/// A query object that includes SQL data from table joins. This
/// object can be later mapped and returned as a flat object to
/// the caller.
/// </summary>
public class AskQueryObject
{
    /// <summary>
    /// Gets or sets the <see cref="DomainAsk"/> object.
    /// </summary>
    public DomainAsk? Ask { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether the user has been by upvoted the <see cref="DomainAsk"/>.
    /// </summary>
    public bool IsUpvoted { get; set; }
}