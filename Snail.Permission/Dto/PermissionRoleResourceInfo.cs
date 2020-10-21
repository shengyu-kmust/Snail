using System.Collections.Generic;

namespace Snail.Permission.Dto
{
    public class PermissionRoleResourceInfo
    {
        public string RoleKey { get; set; }
        public List<string> ResourceKeys { get; set; }
    }
}
