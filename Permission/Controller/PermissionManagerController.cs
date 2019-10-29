using Microsoft.AspNetCore.Mvc;
using Snail.Permission.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Controller
{
    public class PermissionManagerController:ControllerBase
    {
        private IPermissionDataManager permissionDataManager;
        #region 
        public object GetOrgTree()
        {
            throw new NotImplementedException();
        }

        #region 角色-资源
        /// <summary>
        /// 为角色增加的资源权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="resourceIds">资源</param>
        /// <returns></returns>
        public ActionResult AddPermission(object roleId, List<object> resourceIds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移除角色的的资源权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="resourceIds">资源ID</param>
        /// <returns></returns>
        public ActionResult RemovePermission(object roleId, List<object> resourceIds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取角色所有的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ActionResult RoleAllPermission(object roleId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 用户-角色
        /// <summary>
        /// 设置用户的角色
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public ActionResult AddRolesToUser(object userId, object roleIds)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 删除用户的角色
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public ActionResult RemoveRolesFromUsers(int userId, string roleIds)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region 资源


        #endregion

        #region 角色

        public ActionResult AddRole(object roleName)
        {
            throw new NotImplementedException();
        }

        public ActionResult DeleteRole(object roleId)
        {
            throw new NotImplementedException();
        }

        #endregion
        #endregion
    }
}
