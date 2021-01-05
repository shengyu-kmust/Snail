using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Snail.RS
{
    public  static class RSServiceCollectionExtenssion
    {
        /// <summary>
        /// 增加预约service，注意要增加automapper，用RsAutomapperProfile
        /// </summary>
        /// <param name="services"></param>
        public static void AddRs(this IServiceCollection services)
        {
            services.AddTransient<RSService>();
        }
    }
}
