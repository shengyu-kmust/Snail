using Hangfire.Dashboard;

namespace Snail.Web
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpcontext = context.GetHttpContext();
            return httpcontext.User?.Identity?.IsAuthenticated??false;
        }
    }
}
