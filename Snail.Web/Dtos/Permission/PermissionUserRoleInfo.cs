using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Web.Dtos
{
    public class PermissionUserRoleInfo
    {
        public string UserKey { get; set; }
        public List<string> RoleKeys { get; set; }
    }
}
