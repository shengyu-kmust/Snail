using System.Reflection;

namespace Snail.Core.Permission
{
    public class PermissionOptions
    {
        public string PasswordSalt { get; set; } = string.Empty;
        public string RsaPrivateKey { get; set; }
        public string RsaPublicKey { get; set; }
        /// <summary>
        /// 从哪些程序集里初始化resource
        /// </summary>
        public Assembly[] ResourceAssemblies { get; set; }
    }
}
