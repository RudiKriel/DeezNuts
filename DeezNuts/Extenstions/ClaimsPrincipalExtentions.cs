using System.Security.Claims;

namespace DeezNuts.Extenstions
{
    public static class ClaimsPrincipalExtentions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
