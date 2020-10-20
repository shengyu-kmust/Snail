using Snail.Core.Entity;
using Snail.Core.Permission;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snail.Permission.Entity
{
    [Table("RoleResource")]
    public class PermissionDefaultRoleResource : DefaultBaseEntity, IRoleResource
    {
        public string RoleId { get; set; }
        public string ResourceId { get; set; }

        public string GetResourceKey()
        {
            return this.ResourceId;
        }

        public string GetRoleKey()
        {
            return this.RoleId;
        }

        public void SetResourceKey(string resourceKey)
        {
            ResourceId = resourceKey;
        }

        public void SetRoleKey(string roleKey)
        {
            RoleId = roleKey;
        }
    }
}
