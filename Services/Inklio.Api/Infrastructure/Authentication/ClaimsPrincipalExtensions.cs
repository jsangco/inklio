using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static UserId? UserIdOrDefault(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        string? id = idClaim?.Value;
        if (id is null)
        {
            return null;
        }
        return new Guid(id);
    }

    public static UserId UserId(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        var id = idClaim?.Value ?? throw new InvalidOperationException("Attempted to the identity of unauthenticated request.");
        return new Guid(id);
    }

    public static string Username(this ClaimsPrincipal user)
    {
        var username = user.Identity?.Name ?? throw new InvalidOperationException("Attempted to the identity of unauthenticated request.");
        return username;
    }
}