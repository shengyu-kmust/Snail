using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.WeiXin.Models
{
    /// <summary>
    /// accessToken返回结构
    /// </summary>
    /// <remarks>
    /// https请求方式: GET https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET
    /// </remarks>
    public class AccessTokenResult: WxJsonResult
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in { get; set; }
    }
}
