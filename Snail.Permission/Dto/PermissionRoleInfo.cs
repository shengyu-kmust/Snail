using Snail.Core;
using Snail.Core.Permission;

namespace Snail.Permission.Dto
{
    /// <summary>
    /// 权限角色信息，用于默认的权限api dto
    /// </summary>
    public class PermissionRoleInfo:IRole, IIdField<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string GetKey()
        {
            return Id;
        }

        public string GetName()
        {
            return Name;
        }

        public void SetKey(string key)
        {
            this.Id = key;
        }

        public void SetName(string name)
        {
            this.Name = name;
        }
    }
}
