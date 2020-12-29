using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Snail.Core.Default;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Snail.WeiXin.Test
{
    public class OAuthApiTest
    {
        private OAuthApi OAuthApi;
        public OAuthApiTest()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddHttpClient();
            services.AddMemoryCache();
            services.AddSingleton<OAuthApi>();
            services.AddSnailHttpClient();
            var provider = services.BuildServiceProvider();
            OAuthApi=provider.GetService<OAuthApi>();
        }
        [Fact]
        public async Task Test1()
        {
            try
            {
                var token = await OAuthApi.GetAccessToken("wx14b5123d4cb27356", "121dbb62f210e2f33cf70e87423ae9b7");
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        public void Test()
        {
            
        }
    }
}
