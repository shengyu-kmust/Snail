using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Core
{
    /// <summary>
    /// 作为单例
    /// </summary>
    public interface IPermission<TUser,TRole,TResource,TUserRole,TRoleResource>
    {
        string GetUserId(TUser user);
        string GetRoleId(TUser role);
        string GetResourceId(TResource resource); 
        bool HasPermission(TUser user, TResource resource);
        void Init(List<string> users, List<string> roles, List<(string user, string role)> userRoles, List<string> resource, List<(string role, string resource)> roleResource);

    }
}
