using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Permission
{
    ///// <summary>
    ///// 给到前端，资源和角色的关系。以资源为主维度
    ///// </summary>
    //public interface IResourceRoleInfo
    //{
    //    IResource Resource { get; set; }
    //    List<IRole> Roles { get; set; }
    //    //string ResourceKey { get; set; }
    //    //List<string> RoleKeys { get; set; }
    //}
    public class ResourceRoleInfo
    {
        public string ResourceKey { get; set; }
        public string ResourceName { get; set; }
        public string ResourceCode { get; set; }
        public List<string> RoleKeys { get; set; }
    }
}
