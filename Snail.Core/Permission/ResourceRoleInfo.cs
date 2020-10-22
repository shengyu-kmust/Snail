using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Permission
{
    /// <summary>
    /// 资源和角色的关系。以资源为主维度
    /// </summary>
    public class ResourceRoleInfo
    {
        public string ResourceKey { get; set; }
        public string ResourceName { get; set; }
        public string ResourceCode { get; set; }
        public List<string> RoleKeys { get; set; }
    }
}
