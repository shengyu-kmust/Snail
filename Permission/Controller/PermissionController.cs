using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Core.Permission;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Snail.Permission.Controller
{
    [Route("[Controller]/[Action]"), Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    public class PermissionController:ControllerBase
    {
        private IPermission _permission;
        private IPermissionStore _permissionStore;
        public PermissionController(IPermission permission, IPermissionStore permissionStore)
        {
            _permission = permission;
            _permissionStore = permissionStore;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        [HttpPost,AllowAnonymous]
        public LoginResult GetLoginToken(LoginDto loginDto)
        {
            return _permission.Login(loginDto);
        }

        /// <summary>
        /// 获取所有的资源以及资源角色的对应关系信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<IResourceRoleInfo> GetAllResourceRoles()
        {
            return _permission.GetAllResourceRoles();
        }

        /// <summary>
        /// 初始化权限资源。初始化后请注释此方法
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public void InitResource()
        {
            _permissionStore.InitResource();
        }


    }
}
