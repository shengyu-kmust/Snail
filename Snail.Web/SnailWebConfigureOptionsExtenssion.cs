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
        /// <summary>
        /// 从configuration里配置snailWeb框架的所有Option
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigAllOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StaticFileUploadOption>(configuration.GetSection("StaticFileUploadOption"));
            services.Configure<LogInterceptorOption>(configuration.GetSection("LogInterceptorOption"));
            return services;
        }
    }


}
