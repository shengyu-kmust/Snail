using Snail.Core.Entity;
using Snail.Core.Permission;

/// <summary>
/// todo:这个项目里可不要有User,Role这些实体，一来这些命名被这个项目占用，二来可以让调用者自己定义实体，不要去给默认的实体 ，但暂时先不去，等2.0再去
/// </summary>
namespace Snail.Permission.Entity
{
    public class PermissionDefaultOrg : DefaultBaseEntity, IOrg
    {
        public string ParentId { get; set; }
        public string Name { get; set; }

        public string GetKey()
        {
            return this.Id;
        }

        public string GetName()
        {
            return this.Name;
        }

        public void SetName(string name)
        {
            this.Name = name;
        }
    }
}
