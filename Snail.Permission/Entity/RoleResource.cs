using Snail.Core.Entity;
using Snail.Core.Permission;

namespace Snail.Permission.Entity
{
    public class RoleResource: DefaultBaseEntity, IRoleResource
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
    }
}
