using System.Security.Claims;

namespace VulnerableAPI;

public static class ClaimsHelper
{
    public static string GetEmail(this ClaimsPrincipal user)
    {
        return user.Claims.First(x => x.Type == ClaimTypes.Email).Value;
    }

    public static Guid GetId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }
}
