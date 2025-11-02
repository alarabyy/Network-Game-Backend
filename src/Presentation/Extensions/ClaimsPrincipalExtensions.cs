using System.Security.Claims;

namespace Presentation.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userId, out var id) ? id : null;
    }

    public static string? GetRole(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Role)?.Value;
    }
    public static bool Is(this ClaimsPrincipal user, int id) => id == user.GetId();
}