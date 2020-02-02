using System;
using System.Collections.Generic;
using System.Reflection;

namespace Snail.Core.Permission
{
    public class PermissionOptions
    {
        public string PasswordSalt { get; set; } = string.Empty;
        public string RsaPrivateKey { get; set; }
        public string RsaPublicKey { get; set; }
        /// <summary>
        /// 默认用HmacSha256签名，注意key的大小要大于128bits
        /// </summary>
        public string SymmetricSecurityKey { get; set; }
        public bool IsAsymmetric { get; set; }
        public string Issuer { get; set; } = "snail";
        public string Audience { get; set; } = "snail";
        public string AccessDeniedPath { get; set; } = "/forbid";
        public string LoginPath { get; set; } = "/login";
        public TimeSpan ExpireTimeSpan { get; set; } = new TimeSpan(12, 0, 0);
        /// <summary>
        /// 从哪些程序集里初始化resource
        /// </summary>
        public List<Assembly> ResourceAssemblies { get; set; }
    }
}
