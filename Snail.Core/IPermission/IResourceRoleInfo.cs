using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.IPermission
{
    /// <summary>
    /// 给到前端，资源和角色的关系。以资源为主维度
    /// </summary>
    public interface IResourceRoleInfo
    {
        string ResourceKey { get; set; }
        List<string> RoleKeys { get; set; }
    }
}
