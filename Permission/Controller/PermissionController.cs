using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Core.IPermission;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Snail.Permission.Controller
{
    [Route("[Controller]/[Action]"), Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    public class PermissionController:ControllerBase
    {
        private IPermission _permission;
        public PermissionController(IPermission permission)
        {
            _permission = permission;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        [HttpGet,AllowAnonymous]
        public string GetLoginToken([FromQuery]string account, [FromQuery]string password)
        {
            return _permission.GetLoginToken(account, password);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        [Display()]
        public IUserInfo GetUserInfo(string token)
        {
            return _permission.GetUserInfo(token);
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

        [AllowAnonymous]
        [HttpGet]
        public void InitResource()
        {
            _permission.InitResource();
        }


    }
}
