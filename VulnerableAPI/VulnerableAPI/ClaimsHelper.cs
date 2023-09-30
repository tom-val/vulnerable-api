using System.Security.Claims;

namespace VulnerableAPI;

public static class ClaimsHelper
{
    public static string GetEmail(this ClaimsPrincipal user)
    {
        return user.Claims.First(x => x.Type == ClaimTypes.Email).Value;
    }
}
