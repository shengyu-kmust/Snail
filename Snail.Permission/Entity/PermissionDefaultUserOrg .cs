using Snail.Core.Entity;
using Snail.Core.Permission;
using System;

namespace Snail.Permission.Entity
{
    public class PermissionDefaultUserOrg : DefaultBaseEntity, IUserOrg
    {
        public string UserId { get; set; }
        public string OrgId { get; set; }

        public string GetOrgKey()
        {
            return OrgId;
        }

        public string GetUserKey()
        {
            return UserId;
        }
    }
}
