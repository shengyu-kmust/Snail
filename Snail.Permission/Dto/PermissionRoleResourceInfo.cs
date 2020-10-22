using System.Collections.Generic;

namespace Snail.Permission.Dto
{
    /// <summary>
    /// 角色和拥有的资源信息，用于默认的权限api dto
    /// </summary>
    public class PermissionRoleResourceInfo
    {
        public string RoleKey { get; set; }
        public List<string> ResourceKeys { get; set; }
    }
}
