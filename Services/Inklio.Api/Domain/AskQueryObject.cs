namespace Inklio.Api.Domain;

public class AskQueryObject
{
    public DomainAsk? Ask { get; set; }
    public bool IsUpvoted { get; set; }
}