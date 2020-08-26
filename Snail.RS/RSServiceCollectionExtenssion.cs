using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Snail.RS
{
    public  static class RSServiceCollectionExtenssion
    {
        public static void AddRs(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(RSService));

        }
    }
}
