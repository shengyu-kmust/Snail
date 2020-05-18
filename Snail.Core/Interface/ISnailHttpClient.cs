using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
namespace Snail.Core.Interface
{
    /// <summary>
    /// 接口请求的帮助类
    /// </summary>    
    public interface ISnailHttpClient
    {
        Task<TResult> GetAsync<TResult>(string url, Dictionary<string, string> Params = null, string clientName = null);
        Task<TResult> PostAsync<TResult>(string url, object data, string clientName = null);
        Task<TResult> SendAsync<TResult>(RequestData requestData);
    }


    public enum DataType
    {
        Json,
        formUrlencoded
    }

    public class RequestData
    {
        public string Url { get; set; }
        public string ClientName { get; set; }
        public HttpMethod RequestMethod { get; set; }
        public Dictionary<string, string> Params { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public object Data { get; set; }
        public DataType DataType { get; set; } = DataType.Json;

    }
}
