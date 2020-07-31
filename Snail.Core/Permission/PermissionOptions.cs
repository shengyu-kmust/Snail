using System;
using System.Collections.Generic;
using System.Reflection;

namespace Snail.Core.Permission
{
    /// <summary>
    /// 权限组件的配置
    /// </summary>
    public class PermissionOptions
    {
        /// <summary>
        /// 密码加密处理的盐
        /// </summary>
        public string PasswordSalt { get; set; } = string.Empty;
        /// <summary>
        /// rsa的私钥，用于jwt的生成
        /// </summary>
        public string RsaPrivateKey { get; set; }
        /// <summary>
        /// rsa的公钥，用于jwt的验签
        /// </summary>
        public string RsaPublicKey { get; set; }
        /// <summary>
        /// 默认用HmacSha256签名和验签，注意key的大小要大于128bits
        /// </summary>
        public string SymmetricSecurityKey { get; set; }
        /// <summary>
        /// jwt是否为不对称加密，true时用Rsa进行jwt验签，false时用hmacSha256进行jwt验签
        /// </summary>
        public bool IsAsymmetric { get; set; }
        /// <summary>
        /// 颁发人
        /// </summary>
        public string Issuer { get; set; } = "snail";
        /// <summary>
        /// 受众
        /// </summary>
        public string Audience { get; set; } = "snail";
        /// <summary>
        /// 拒绝访问时的跳转地址
        /// </summary>
        public string AccessDeniedPath { get; set; } = "/forbid";
        /// <summary>
        /// 登录地址，如果用户未登录，会跳转到此地址
        /// </summary>
        public string LoginPath { get; set; } = "/login";
        /// <summary>
        /// token的失效时长
        /// </summary>
        public TimeSpan ExpireTimeSpan { get; set; } = new TimeSpan(12, 0, 0);
        /// <summary>
        /// 从哪些程序集里初始化resource
        /// </summary>
        public List<Assembly> ResourceAssemblies { get; set; }
    }
}
