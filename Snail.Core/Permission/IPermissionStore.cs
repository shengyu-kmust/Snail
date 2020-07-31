using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Permission
{
    /// <summary>
    /// 权限存储相关的接口约定
    /// </summary>
    public interface IPermissionStore
    {
        #region 查询权限数据
        /// <summary>
        /// 获取所有的用户
        /// </summary>
        /// <returns></returns>
        List<IUser> GetAllUser();
        /// <summary>
        /// 获取所有的角色
        /// </summary>
        /// <returns></returns>
        List<IRole> GetAllRole();
        /// <summary>
        /// 获取所有角色和用户的关系
        /// </summary>
        /// <returns></returns>
        List<IUserRole> GetAllUserRole();
        /// <summary>
        /// 获取所有的资源
        /// </summary>
        /// <returns></returns>
        List<IResource> GetAllResource();
        /// <summary>
        /// 获取所有角色和资源的关系
        /// </summary>
        /// <returns></returns>
        List<IRoleResource> GetAllRoleResource();
        #endregion

        #region 管理权限数据

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="user"></param>
        void SaveUser(IUser user);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userKey"></param>
        void RemoveUser(string userKey);
        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="role"></param>
        void SaveRole(IRole role);
        /// <summary>
        /// 删除角色 
        /// </summary>
        /// <param name="roleKey"></param>
        void RemoveRole(string roleKey);
        /// <summary>
        /// 保存资源
        /// </summary>
        /// <param name="resource"></param>
        void SaveResource(IResource resource);
        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="resourceKey"></param>
        void RemoveResource(string resourceKey);
        /// <summary>
        /// 设备用户的角色
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="roleKeys"></param>
        void SetUserRoles(string userKey, List<string> roleKeys);
        /// <summary>
        /// 设置角色的资源
        /// </summary>
        /// <param name="roleKey">角色key</param>
        /// <param name="resourceKeys">资源keys</param>
        void SetRoleResources(string roleKey, List<string> resourceKeys);

        /// <summary>
        /// IPermissionStore的实现里如果用了缓存，此方法用于刷新缓存为最新数据。
        /// 如果用户是通过非IPermissionStore接口方法操作权限数据，则要调用此方法进行数据刷新 
        /// </summary>
        void ReloadPemissionDatas();
        #endregion

    }
}
