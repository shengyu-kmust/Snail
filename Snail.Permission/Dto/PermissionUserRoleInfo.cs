using System.Collections.Generic;

namespace Snail.Permission.Dto
{
    /// <summary>
    ///  用户和用户拥有的角色信息，用于默认的权限api dto
    /// </summary>
    public class PermissionUserRoleInfo
    {
        public string UserKey { get; set; }
        public List<string> RoleKeys { get; set; }
    }
}
