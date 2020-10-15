using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snail.Web.Controllers;
using Snail.Web.Interceptor;


namespace Snail.Web
{
    public static class SnailWebConfigureServicesExtenssions
    {
        public static IServiceCollection ConfigAllOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StaticFileUploadOption>(configuration.GetSection("StaticFileUploadOption"));
            services.Configure<LogInterceptorOption>(configuration.GetSection("LogInterceptorOption"));
            return services;
        }
    }
}
