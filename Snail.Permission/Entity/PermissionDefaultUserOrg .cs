using Snail.Core.Entity;
using Snail.Core.Permission;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snail.Permission.Entity
{
    [Table("UserOrg")]
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
