using Microsoft.AspNetCore.Mvc;
using Snail.Permission.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Controller
{
    public class PermissionController:ControllerBase
    {
        private IPermissionManager _permissionManager;

        #region 账号登录注册相关
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Token(string account, string password)
        {
            return _permissionManager.GetToken(account, password);
        }

        /// <summary>
        /// 登录，登录信息放入cockies
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void Login(string account, string password)
        {
            _permissionManager.SignIn(account, password);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public void Registor(string account,string password)
        {
            _permissionManager.Registor(account, password);
        }

        #endregion



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
