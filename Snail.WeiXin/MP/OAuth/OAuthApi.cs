using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Snail.Core;
using Snail.Core.Interface;
using Snail.WeiXin.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Snail.WeiXin
{
    public class OAuthApiOption
    {
        public string ApiMpHost { get; set; } = "https://api.weixin.qq.com";
    }
    /// <summary>
    /// 代公众号发起网页授权，参考Senparc的方法
    /// </summary>
    public class OAuthApi
    {
        private ISnailHttpClient _snailHttpClient;
        private IOptionsMonitor<OAuthApiOption> _optionsMonitor;
        private IMemoryCache _memoryCache;
        public OAuthApi(ISnailHttpClient snailHttpClient, IOptionsMonitor<OAuthApiOption> optionsMonitor, IMemoryCache memoryCache)
        {
            _snailHttpClient = snailHttpClient;
            _optionsMonitor = optionsMonitor;
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// 获取验证地址
        /// </summary>
        /// <param name="appId">公众号的appid</param>
        /// <param name="componentAppId">第三方平台的appid</param>
        /// <param name="redirectUrl">重定向地址，需要urlencode，这里填写的应是服务开发方的回调地址</param>
        /// <param name="state">重定向后会带上state参数，开发者可以填写任意参数值，最多128字节</param>
        /// <param name="scope">授权作用域，拥有多个作用域用逗号（,）分隔。此处暂时只放一作用域。</param>
        /// <param name="responseType">默认，填code</param>
        /// <returns></returns>
        public string GetAuthorizeUrl(string appId, string componentAppId, string redirectUrl, string state, string scope, string responseType = "code")
        {
            //此URL比MP中的对应接口多了&component_appid=component_appid参数
            var url =
                string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}&component_appid={5}#wechat_redirect",
                                HttpUtility.UrlEncode(appId), HttpUtility.UrlEncode(redirectUrl), HttpUtility.UrlEncode(responseType), HttpUtility.UrlEncode(scope), HttpUtility.UrlEncode(state), HttpUtility.UrlEncode(componentAppId));

            /* 这一步发送之后，客户会得到授权页面，无论同意或拒绝，都会返回redirectUrl页面。
             * 如果用户同意授权，页面将跳转至 redirect_uri?code=CODE&state=STATE&appid=APPID。这里的code用于换取access_token（和通用接口的access_token不通用）
             * 若用户禁止授权，则重定向后不会带上code参数，仅会带上state参数redirect_uri?state=STATE
             */
            return url;
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="componentAppid">服务开发方的appid</param>
        /// <param name="componentAccessToken">服务开发方的access_token</param>
        /// <param name="code">GetAuthorizeUrl()接口返回的code</param>
        /// <param name="grantType"></param>
        /// <returns></returns>
        public async Task<OAuthAccessTokenResult> GetAccessToken(string appId, string componentAppid, string componentAccessToken, string code, string grantType = "authorization_code")
        {
            var url =
                string.Format(_optionsMonitor.CurrentValue.ApiMpHost + "/sns/oauth2/component/access_token?appid={0}&code={1}&grant_type={2}&component_appid={3}&component_access_token={4}",
                                HttpUtility.UrlEncode(appId), HttpUtility.UrlEncode(code), HttpUtility.UrlEncode(grantType), HttpUtility.UrlEncode(componentAppid), HttpUtility.UrlEncode(componentAccessToken));
            return await _snailHttpClient.GetAsync<OAuthAccessTokenResult>(url);
        }

        /// <summary>
        /// 刷新access_token（如果需要）
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="refreshToken">填写通过access_token获取到的refresh_token参数</param>
        /// <param name="componentAccessToken"></param>
        /// <param name="grantType"></param>
        /// <param name="componentAppid"></param>
        /// <returns></returns>
        public async Task<OAuthAccessTokenResult> RefreshToken(string appId, string refreshToken, string componentAppid, string componentAccessToken, string grantType = "refresh_token")
        {
            var url = string.Format(_optionsMonitor.CurrentValue.ApiMpHost + "/sns/oauth2/component/refresh_token?appid={0}&grant_type={1}&component_appid={2}&component_access_token={3}&refresh_token={4}",
                                HttpUtility.UrlEncode(appId), HttpUtility.UrlEncode(grantType), HttpUtility.UrlEncode(componentAppid), HttpUtility.UrlEncode(componentAccessToken), HttpUtility.UrlEncode(refreshToken));

            return await _snailHttpClient.GetAsync<OAuthAccessTokenResult>(url);
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="openId">普通用户的标识，对当前公众号唯一</param>
        /// <param name="lang">返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语</param>
        /// <returns></returns>
        public async Task<OAuthUserInfo> GetUserInfo(string accessToken, string openId, Language lang = Language.zh_CN)
        {
            var url = string.Format(_optionsMonitor.CurrentValue.ApiMpHost + "/sns/userinfo?access_token={0}&openid={1}&lang={2}", HttpUtility.UrlEncode(accessToken), HttpUtility.UrlEncode(openId), lang);
            return await _snailHttpClient.GetAsync<OAuthUserInfo>(url);
        }

        /// <summary>
        /// 获取appid的接口请求access_token
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public async Task<string> GetAccessToken(string appId, string appSecret)
        {
            var token=await _memoryCache.GetOrCreate($"{appId}_accessToken", async entry =>
             {
                 var accessUrl = "https://api.weixin.qq.com/cgi-bin/token";
                 var res = await _snailHttpClient.GetAsync<AccessTokenResult>(accessUrl, new Dictionary<string, string>()
                 {
                    {"appid",appId },
                    {"secret",appSecret },
                    {"grant_type","client_credential" }
                 });
                 if (!string.IsNullOrEmpty(res.access_token))
                 {
                     var tokenStr = res.access_token;
                     entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(res.expires_in);
                     return tokenStr;
                 }
                 else
                 {
                     throw new BusinessException($"获取access_token失败，失败返回：{JsonConvert.SerializeObject(res)}");
                 }

               
             });
            return token;
        }


    }

}
