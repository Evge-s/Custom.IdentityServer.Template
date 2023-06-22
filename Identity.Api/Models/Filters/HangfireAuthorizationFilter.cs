using System.Security.Claims;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Identity.Api.Models.Filters;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var user = httpContext.User;

        // Allow only if the user is authenticated and has the "Admin" claim
        return user.Identity != null 
               && user.Identity.IsAuthenticated 
               && user.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "GeneralAdmin");
    }
}