using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Dto
{
    public class SetUserRoleDto
    {
        public string UserKey { get; set; }
        public List<string> RoleKeys { get; set; }
    }
}
