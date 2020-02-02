using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Permission
{
    public interface IPermissionStore
    {
        #region 查询权限数据
        List<IUser> GetAllUser();
        List<IRole> GetAllRole();
        List<IUserRole> GetAllUserRole();
        List<IResource> GetAllResource();
        List<IRoleResource> GetAllRoleResource();
        #endregion

        #region 管理权限数据

        void SaveUser(IUser user);
        void RemoveUser(string userKey);
        void SaveRole(IRole role);
        void RemoveRole(string roleKey);
        void SaveResource(IResource resource);
        void RemoveResource(string resourceKey);
        /// <summary>
        /// 设备用户的角色
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="roleKeys"></param>
        void SetUserRoles(string userKey, List<string> roleKeys);
        void SetRoleResources(string roleKey, List<string> resourceKeys);

        /// <summary>
        /// IPermissionStore的实现里如果用了缓存，此方法用于刷新缓存为最新数据。
        /// 如果用户是通过非IPermissionStore接口方法操作权限数据，则要调用此方法进行数据刷新 
        /// </summary>
        void ReloadPemissionDatas();
        #endregion

    }
}
