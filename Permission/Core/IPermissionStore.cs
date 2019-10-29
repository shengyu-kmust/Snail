using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Core
{
    /// <summary>
    /// 构成权限功能的数据有：用户、角色、用户角色的关系、资源、角色和资源的关系
    /// </summary>
    public interface IPermissionStore
    {
        List<string> GetUserKeys();
        List<string> GetRoleKeys();
        List<(string userKey, string roleKey)> GetUserRoles();
        List<string> GetResourceKeys();
        List<(string roleKey, List<string> roleResources)> GetRoleResources();
    }
}
