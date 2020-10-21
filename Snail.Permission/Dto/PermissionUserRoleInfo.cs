using System.Collections.Generic;

namespace Snail.Permission.Dto
{
    public class PermissionUserRoleInfo
    {
        public string UserKey { get; set; }
        public List<string> RoleKeys { get; set; }
    }
}
