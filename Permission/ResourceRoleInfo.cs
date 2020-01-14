using Snail.Core.IPermission;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission
{
    public class ResourceRoleInfo : IResourceRoleInfo
    {
        public string ResourceKey { get;set; }
        public List<string> RoleKeys { get;set; }
    }
}
 