using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snail.Web.Controllers;
using Snail.Web.Interceptor;


namespace Snail.Web
{
    /// <summary>
    /// SnailWebConfigureOptionsExtenssion
    /// </summary>
    public static class SnailWebConfigureOptionsExtenssion
    {
        public static IServiceCollection ConfigAllOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StaticFileUploadOption>(configuration.GetSection("StaticFileUploadOption"));
            services.Configure<LogInterceptorOption>(configuration.GetSection("LogInterceptorOption"));
            return services;
        }
    }


}
