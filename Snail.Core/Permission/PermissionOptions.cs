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
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string AccessDeniedPath { get; set; }
        public string LoginPath { get; set; }
        public TimeSpan ExpireTimeSpan { get; set; }
        /// <summary>
        /// 从哪些程序集里初始化resource
        /// </summary>
        public List<Assembly> ResourceAssemblies { get; set; }
    }
}
