using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace Snail.Core.Default
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 此类的实现用了IHttpClientFactory
    /// 对于要有授权或是增加httpclient的header的接口，请通过IServiveCollection.AddHttpClient先配置具体httpClient的参数
    /// 
    /// </remarks>
    public class SnailHttpClient : ISnailHttpClient
    {
        private IHttpClientFactory _httpClientFactory;
        public SnailHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TResult> GetAsync<TResult>(string url, Dictionary<string, string> Params = null, string clientName = null)
        {
            return await SendAsync<TResult>(new RequestData
            {
                Url = url,
                Params = Params,
                ClientName = clientName,
                RequestMethod = HttpMethod.Get
            });
        }

        public async Task<TResult> PostAsync<TResult>(string url, object data, string clientName = null)
        {
            return await SendAsync<TResult>(new RequestData
            {
                Url = url,
                ClientName = clientName,
                RequestMethod = HttpMethod.Post,
                DataType = DataType.Json
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public async Task<TResult> SendAsync<TResult>(RequestData requestData)
        {
            var paramsString = string.Empty;
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = requestData.RequestMethod
            };
            if (requestData.Params != null && requestData.Params.Count > 0)
            {
                paramsString = string.Join("&", requestData.Params.Select(a => $"{a.Key}={a.Value}"));
            }
            if (requestData.Headers != null && requestData.Headers.Count > 0)
            {
                foreach (var item in requestData.Headers)
                {
                    httpRequestMessage.Headers.TryAddWithoutValidation(item.Key, item.Value);
                }
            }

            if (requestData.DataType == DataType.formUrlencoded)
            {
                if (requestData.Data is IEnumerable<KeyValuePair<string, string>> formUrlData)
                {
                    httpRequestMessage.Content = new FormUrlEncodedContent(formUrlData);
                }
                else
                {
                    throw new Exception($"{nameof(requestData.DataType)}={nameof(DataType.formUrlencoded)}时，{nameof(requestData.Data)}必须为IEnumerable<KeyValuePair<string,string>>类型");
                }
            }
            else if (requestData.DataType == DataType.Json)
            {
                httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestData.Data), Encoding.UTF8, "application/json");
            }
            httpRequestMessage.RequestUri = CreateUri($"{requestData.Url.TrimEnd('/')}?{paramsString.TrimStart('/')}");
            using (var client = _httpClientFactory.CreateClient(requestData.ClientName ?? string.Empty))
            {
                var response = await client.SendAsync(httpRequestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResult>(responseString);
                }
                else
                {
                    throw new Exception("请求出错" + response.ToString());
                }

            }

        }

        private Uri CreateUri(string uri) =>
           string.IsNullOrEmpty(uri) ? null : new Uri(uri, UriKind.RelativeOrAbsolute);
    }

    public static class SnailHttpClientServiceExtenssion
    {
        public static void AddSnailHttpClient(this IServiceCollection services)
        {
            services.TryAddScoped<ISnailHttpClient, SnailHttpClient>();
            services.AddHttpClient();
        }
    }


    #region 示例
    //    IServiceCollection serviceCollection = new ServiceCollection();
    //    serviceCollection.AddSnailHttpClient();
    //                serviceCollection.AddHttpClient("opms",config=> {
    //                    config.BaseAddress = new Uri("http://localhost:5000");
    //    var token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJPclBNUyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJsaWhvbmdqaWFuZyIsImp0aSI6ImNjODkyNWFlLWYyNDQtNGVkMC1hMmNlLTE4YmMyMzU3NTFiNSIsImlhdCI6Ii0yMTQ3NDgzIiwibmJmIjoxNTgyNTk4ODc2LCJleHAiOjE1ODI2NDIwNzYsImlzcyI6Ik9yUE1TIiwiYXVkIjoiT3JQTVMifQ.Ob9wCOoC4EGZy-cfeNMYSRTWHFuBYYt4Op4m4m_a81DVqFjBLxLs_ozaVESz9jG9AQnYCx4OBPSYr7MEITFNttj6wfv4qXYOJ4OpwKJaz5P7YJG74dDQlsSafgLe4sPsInAx5tiob1w523WeCdQW647OCG_u3BdRAJu7aMAgyucgFmoK1TbFAHGEhaX9PDSP8UUeQ3qIBTpAqqpjVUITWqh2M6e3DglEiz23g9MWGzl_x5Lhjsq8ea-v6_p7Eb-Im-wUPH_Q6SJhkzUjEpbyaAUJNd3041KveMu6pRs5gggezEaYRycizFqw6ZxkduNcdPwLfKxOwjEWiQEBpRef7w";
    //    config.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //});

    //                var serviceProvider = serviceCollection.BuildServiceProvider();
    //var client = serviceProvider.GetService<ISnailHttpClient>();
    //var result1 = client.GetAsync<ReturnModel<MyPageList<AirportResultDto>>>("http://localhost:5000/api/v1/OrPMS/Airport/Query", new Dictionary<string, string> { { "isDeleted", "false" } }
    //, "opms").Result;
    //var result2 = client.PostAsync<ReturnModel<string>>("api/v1/OrPMS/Airport/Add", new AirportSaveDto(), "opms").Result;
    #endregion

}
