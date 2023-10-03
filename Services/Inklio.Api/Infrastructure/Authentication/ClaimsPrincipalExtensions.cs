using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Gets the UserId of the authenticated user and returns null if the user is not authenticated.
    /// </summary>
    /// <param name="user">The user</param>
    /// <returns>The UserId of the authenticated user or null if the user is not authenticated.</returns>
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

    /// <summary>
    /// Gets the UserId of the authenticated user.
    /// </summary>
    /// <param name="user">The user</param>
    /// <returns>The UserId of the authenticated user.</returns>
    public static UserId UserId(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        var id = idClaim?.Value ?? throw new InvalidOperationException("Attempted to get the identity of unauthenticated request.");
        return new Guid(id);
    }

    /// <summary>
    /// Gets the username of the authenticated user.
    /// </summary>
    /// <param name="user">The user</param>
    /// <returns>The username of the authenticated user.</returns>
    public static string Username(this ClaimsPrincipal user)
    {
        var username = user.Identity?.Name ?? throw new InvalidOperationException("Attempted to get the identity of unauthenticated request.");
        return username;
    }

    /// <summary>
    /// Returns a flag indicating if the user has moderator privileges
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>True if the user has moderator privileges</returns>
    public static bool IsModerator(this ClaimsPrincipal user)
    {
        return user.IsInRole(UserRoles.Administrator) || user.IsInRole(UserRoles.Moderator);
    }
}