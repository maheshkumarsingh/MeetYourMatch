using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserName(this ClaimsPrincipal user)
    {
        var userName = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Cannot get user name from token");
        return userName;
    }
}
