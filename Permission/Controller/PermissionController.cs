using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Core.Attributes;
using Snail.Core.Permission;
using Snail.Permission.Dto;
using Snail.Permission.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Snail.Permission.Controller
{
    [Route("[Controller]/[Action]"), Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy),Resource(Description ="权限管理")]
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
        public LoginResult Login(LoginDto loginDto)
        {
            return _permission.Login(loginDto);
        }

        /// <summary>
        /// 获取所有的资源以及资源角色的对应关系信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public List<ResourceRoleInfo> GetAllResourceRoles()
        {
            return _permission.GetAllResourceRoles();
        }

        /// <summary>
        /// 初始化权限资源
        /// </summary>
        [HttpGet,Resource(Description = "初始化权限资源")]
        public void InitResource()
        {
            _permission.InitResource();
        }

        [HttpPost, Resource(Description = "保存用户")]
        public void SaveUser(User user)
        {
            _permissionStore.SaveUser(user);
        }
        [HttpPost, Resource(Description = "删除用户")]
        public void RemoveUser(string userKey)
        {
            _permissionStore.RemoveUser(userKey);
        }

        [HttpPost, Resource(Description = "保存角色")]
        public void SaveRole(Role role)
        {
            _permissionStore.SaveRole(role);
        }

        [HttpPost, Resource(Description = "删除角色")]
        public void RemoveRole(string roleKey)
        {
            _permissionStore.RemoveRole(roleKey);
        }

        [HttpPost, Resource(Description = "用户授予角色")]
        public void SetUserRoles(SetUserRoleDto dto)
        {
            _permissionStore.SetUserRoles(dto.UserKey,dto.RoleKeys);
        }
        [HttpPost,Resource(Description = "角色授予资源")]
        public void SetRoleResources(SetRoleResourceDto dto)
        {
            _permissionStore.SetRoleResources(dto.RoleKey,dto.ResourceKeys);
        }
    }
}
