using System.Collections.Generic;

namespace Snail.Core.Permission
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo : IUserInfo,ITenant<string>
    {
        public string UserKey { get;set; }
        public string UserName { get;set; }
        public string Account { get;set; }
        public List<string> RoleKeys { get;set; }
        public List<string> RoleNames { get;set; }
        public string TenantId { get; set; }
    }
}
