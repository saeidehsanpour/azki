using System.Security.Claims;

namespace PerformanceManagementSystem;

public static class Tools
{
    public static class Roles
    {
        public const string Administrator = "Administrator";
        public const string President = "President";
    }

    public static Guid UserId(this ClaimsPrincipal claimsPrincipal)
    {
        return Guid.Parse((ReadOnlySpan<char>)claimsPrincipal.Claims.First(a => a.Type == ClaimTypes.NameIdentifier).Value);
    }

    public static string ActivePerformanceManagementPeriod(HttpContext httpContext, Guid id)
    {
        var timeId = httpContext.Request.Cookies["PerformanceManagementCookie"];

        if (timeId != null && Guid.Parse(timeId) == id)
        {
            return "nav-link active";
        }

        return "nav-link text-dark";
    }
}