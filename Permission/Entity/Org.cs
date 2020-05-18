using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// todo:这个项目里可不要有User,Role这些实体，一来这些命名被这个项目占用，二来可以让调用者自己定义实体，不要去给默认的实体 ，但暂时先不去，等2.0再去
/// </summary>
namespace Snail.Permission.Entity
{
    public class Org : BaseEntity
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
    }
}
