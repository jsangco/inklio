using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// An flag for a <see cref="Ask"/> 
/// </summary>
public class AskFlag : Flag
{
    /// <summary>
    /// Gets the parent <see cref="Ask"/> parent object ID.
    /// </summary>
    public int AskId { get; private set; }

    /// <summary>
    /// Gets the parent <see cref="Ask"/> object.
    /// </summary>
    public Ask Ask { get; private set; }

    /// <summary>
    /// Initilaizes an instance of a <see cref="AskFlag"/>
    /// </summary>
#pragma warning disable CS8618
    private AskFlag() : base()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initilaizes an instance of a <see cref="AskFlag"/>
    /// </summary>
    /// <param name="ask">The parent <see cref="Ask"/> object</param>
    /// <param name="typeId">The type of the Flag</param>
    /// <param name="createdBy">The Flag creator</param>
    public AskFlag(Ask ask, int typeId, User user) : base(typeId, user)
    {
        this.Ask = ask;
        this.AskId = ask.Id;
    }
}