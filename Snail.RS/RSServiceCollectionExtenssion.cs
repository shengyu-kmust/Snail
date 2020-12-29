using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Snail.RS
{
    public  static class RSServiceCollectionExtenssion
    {
        public static void AddRs(this IServiceCollection services)
        {
            // addAutoMapper只能用一次，不要用这个方法，待重构
            services.AddAutoMapper(typeof(RSService));

        }
    }
}
