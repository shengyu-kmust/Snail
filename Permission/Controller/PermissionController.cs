﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Core.Attributes;
using Snail.Core.Permission;
using Snail.Permission.Dto;
using Snail.Permission.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Snail.Common.Extenssions;
using System.Linq;

namespace Snail.Permission.Controller
{
    [ApiController]
    [Route("[Controller]/[Action]"), Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy), Resource(Description = "权限管理")]
    public class PermissionController : ControllerBase
    {
        private IPermission _permission;
        private IPermissionStore _permissionStore;
        public PermissionController(IPermission permission, IPermissionStore permissionStore)
        {
            _permission = permission;
            _permissionStore = permissionStore;
        }

        #region 查询权限数据
        [HttpGet, Resource(Description = "查询所有用户")]
        public List<UserDto> GetAllUser()
        {
            var allUser = _permissionStore.GetAllUser().Select(a => (User)a).Select(a => new UserDto
            {
                Account = a.Account,
                Creater = a.Creater,
                CreateTime = a.CreateTime,
                Email = a.Email,
                Gender = a.Gender,
                Id = a.Id,
                IsDeleted = a.IsDeleted,
                Name = a.Name,
                Phone = a.Phone,
                Pwd = "******",
                Updater = a.Updater,
                UpdateTime = a.UpdateTime
            }).ToList();
            allUser.ForEach(item =>
            {
                item.CreaterName = allUser.FirstOrDefault(a => a.Id == item.Creater)?.Name;
                item.UpdaterName = allUser.FirstOrDefault(a => a.Id == item.Updater)?.Name;
            });
            return allUser.Where(a => !a.IsDeleted).ToList();
        }
        [HttpGet, Resource(Description = "查询所有角色")]
        public List<RoleDto> GetAllRole()
        {
            var allUser = _permissionStore.GetAllUser().Select(a => new { Id = a.GetKey(), Name = a.GetName() });
            var allRole = _permissionStore.GetAllRole().Select(a => (Role)a).Select(a => new RoleDto
            {
                Creater = a.Creater,
                CreateTime = a.CreateTime,
                Id = a.Id,
                IsDeleted = a.IsDeleted,
                Name = a.Name,
                Updater = a.Updater,
                UpdateTime = a.UpdateTime
            }).ToList();
            allRole.ForEach(item =>
            {
                item.CreaterName = allUser.FirstOrDefault(a => a.Id == item.Creater)?.Name;
                item.UpdaterName = allUser.FirstOrDefault(a => a.Id == item.Updater)?.Name;
            });
            return allRole.Where(a => !a.IsDeleted).ToList();
        }

        [HttpGet, Resource(Description = "查询用户的所有角色")]
        public UserRoleDto GetUserRoles(string userKey)
        {
            var userRoleKeys = _permissionStore.GetAllUserRole().Where(a => a.GetUserKey() == userKey).Select(a => a.GetRoleKey()).Distinct().ToList();
            return new UserRoleDto
            {
                UserKey = userKey,
                RoleKeys = userRoleKeys
            };
        }

        [HttpGet, Resource(Description = "查询角色的所有资源")]
        public RoleResourceDto GetRoleResources(string roleKey)
        {
            var roleResourceKeys = _permissionStore.GetAllRoleResource().Where(a => a.GetRoleKey() == roleKey).Select(a => a.GetResourceKey()).Distinct().ToList();
            return new RoleResourceDto
            {
                RoleKey = roleKey,
                ResourceKeys = roleResourceKeys
            };
        }
        #endregion
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public LoginResult Login(LoginDto loginDto)
        {
            var result = _permission.Login(loginDto);
            if (loginDto.SignIn)
            {
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(_permission.GetClaims(result.UserInfo), CookieAuthenticationDefaults.AuthenticationScheme, PermissionConstant.userIdClaim, PermissionConstant.roleIdsClaim));
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            }
            return result;
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
        [HttpGet, Resource(Description = "初始化权限资源")]
        public void InitResource()
        {
            _permission.InitResource();
        }

        [HttpPost, Resource(Description = "保存用户")]
        public void SaveUser(UserSaveDto user)
        {
            // 增加时，设置密码
            if (user.Id.HasNotValue())
            {
                user.Pwd = user.Pwd.HasValue() ? _permission.HashPwd(user.Pwd) : _permission.HashPwd("123456");
            }
            else
            {
                // 修改时，如果密码不为空，则更新密码
                if (user.Id.HasValue() && user.Pwd.HasValue())
                {
                    user.Pwd = _permission.HashPwd(user.Pwd);
                }
            }
            _permissionStore.SaveUser(new User
            {
                Account = user.Account,
                Email = user.Email,
                Gender = user.Gender,
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Pwd = user.Pwd,

            });
        }
        [HttpPost, Resource(Description = "删除用户")]
        public void RemoveUser(string userKey)
        {
            _permissionStore.RemoveUser(userKey);
        }

        [HttpPost, Resource(Description = "保存角色")]
        public void SaveRole(RoleSaveDto role)
        {
            _permissionStore.SaveRole(new Role
            {
                Id = role.Id,
                Name = role.Name
            });
        }

        [HttpPost, Resource(Description = "删除角色")]
        public void RemoveRole(string roleKey)
        {
            _permissionStore.RemoveRole(roleKey);
        }

        [HttpPost, Resource(Description = "用户授予角色")]
        public void SetUserRoles(UserRoleDto dto)
        {
            _permissionStore.SetUserRoles(dto.UserKey, dto.RoleKeys);
        }
        [HttpPost, Resource(Description = "角色授予资源")]
        public void SetRoleResources(RoleResourceDto dto)
        {
            _permissionStore.SetRoleResources(dto.RoleKey, dto.ResourceKeys);
        }
    }
}
