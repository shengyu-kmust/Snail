using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
namespace Snail.Core.Interface
{
    /// <summary>
    /// 接口请求的帮助类，基于httpclient封装
    /// </summary>    
    public interface ISnailHttpClient
    {
        /// <summary>
        /// 发送get请求，并返回结果
        /// </summary>
        /// <typeparam name="TResult">返回结果的类型</typeparam>
        /// <param name="url">请求url地址，可以是绝对地址或是相对地址，如果是相对地址，要提前配置好httpclient</param>
        /// <param name="Params">请求的参数</param>
        /// <param name="clientName">httpclient名</param>
        /// <returns>返回结果</returns>
        Task<TResult> GetAsync<TResult>(string url, Dictionary<string, string> Params = null, string clientName = null);

        /// <summary>
        /// 发送get请求，并返回结果
        /// </summary>
        /// <typeparam name="TResult">返回结果的类型</typeparam>
        /// <param name="url">请求url地址，可以是绝对地址或是相对地址，如果是相对地址，要提前配置好httpclient</param>
        /// <param name="data">请求的参数，会json序列化后放入到body里</param>
        /// <param name="clientName">httpclient名</param>
        /// <returns>返回结果</returns>
        Task<TResult> PostAsync<TResult>(string url, object data, string clientName = null);

        /// <summary>
        /// 发送http请求，请求的方法和参数由requestData定义
        /// </summary>
        /// <typeparam name="TResult">返回结果的类型</typeparam>
        /// <param name="requestData">请求的参数配置</param>
        /// <returns>返回结果</returns>
        Task<TResult> SendAsync<TResult>(RequestData requestData);
    }


    public enum DataType
    {
        Json,
        formUrlencoded
    }

    public class RequestData
    {
        /// <summary>
        /// 请求地址，如果是相对地址的话，要提前对httpclient的baseuri做设置
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// httpclient名
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// 请求方法
        /// </summary>
        public HttpMethod RequestMethod { get; set; }
        /// <summary>
        /// 放入到url里的请求参数
        /// </summary>
        public Dictionary<string, string> Params { get; set; }
        /// <summary>
        /// 请求头
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }
        /// <summary>
        /// DataType为formUrlencoded时必须为IEnumerable<KeyValuePair<string,string>>
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 请求参数放入到http里的类型，如 Json,formUrlencoded
        /// </summary>
        public DataType DataType { get; set; } = DataType.Json;

    }
}
