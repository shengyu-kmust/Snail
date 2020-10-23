using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Snail.Common.Extenssions;
using Snail.Core.Attributes;
using Snail.Core.Permission;
using Snail.Permission;
using Snail.Permission.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Snail.Web.Controllers
{
    [ApiController]
    [Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    [Resource(Description = "权限管理")]
    [Route("api/[Controller]/[Action]")]
    public class PermissionController : ControllerBase

    {
        private IPermission _permission;
        private IPermissionStore _permissionStore;
        private IToken _token;
        public PermissionController(IPermission permission, IPermissionStore permissionStore,IToken token)
        {
            _permission = permission;
            _permissionStore = permissionStore;
            _token = token;
        }
        #region 特殊化的
        #endregion

        #region 查询权限数据
        [HttpGet, Resource(Description = "查询所有用户")]
        public List<PermissionUserInfo> GetAllUserInfo()
        {
            return _permissionStore.GetAllUser().Select(a => new PermissionUserInfo
            {
                Id = a.GetKey(),
                Account = a.GetAccount(),
                Name = a.GetName(),
            }).ToList();
        }
        [HttpGet, Resource(Description = "查询所有角色")]
        public List<PermissionRoleInfo> GetAllRole()
        {
            return _permissionStore.GetAllRole().Select(a => new PermissionRoleInfo
            {
                Id = a.GetKey(),
                Name = a.GetName(),
            }).ToList();
        }

        [HttpGet, Resource(Description = "查询用户的所有角色")]
        public PermissionUserRoleInfo GetUserRoles(string userKey)
        {
            var userRoleKeys = _permissionStore.GetAllUserRole().Where(a => a.GetUserKey() == userKey).Select(a => a.GetRoleKey()).Distinct().ToList();
            return new PermissionUserRoleInfo
            {
                UserKey = userKey,
                RoleKeys = userRoleKeys
            };
        }

        [HttpGet, Resource(Description = "查询角色的所有资源")]
        public PermissionRoleResourceInfo GetRoleResources(string roleKey)
        {
            var roleResourceKeys = _permissionStore.GetAllRoleResource().Where(a => a.GetRoleKey() == roleKey).Select(a => a.GetResourceKey()).Distinct().ToList();
            return new PermissionRoleResourceInfo
            {
                RoleKey = roleKey,
                ResourceKeys = roleResourceKeys
            };
        }
        #endregion

        #region 登录注销
        /// <summary>
        /// 登录并获取token
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
        /// 注销
        /// </summary>
        [HttpPost, AllowAnonymous]
        public void Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        #endregion

        #region 获取用户信息
        /// <summary>
        /// 根据token获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public UserInfo GetUserInfo(string token)
        {
            var claims = _token.ResolveFromToken(token);
            return _permission.GetUserInfo(new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, PermissionConstant.userIdClaim, PermissionConstant.roleIdsClaim)));
        }

        /// <summary>
        /// 获取当前已登录用户信息
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public UserInfo GetCurrentLoginUserInfo()
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values))
            {
                var token = (values.FirstOrDefault() ?? "").Replace("Bearer ", "");
                return GetUserInfo(token);
            }
            else
            {
                return _permission.GetUserInfo(new ClaimsPrincipal(new ClaimsIdentity(HttpContext.User.Claims, CookieAuthenticationDefaults.AuthenticationScheme, PermissionConstant.userIdClaim, PermissionConstant.roleIdsClaim)));
            }
        }
        #endregion

        /// <summary>
        /// 获取所有的资源以及资源角色的对应关系信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public List<ResourceRoleInfo> GetAllResourceRoles()
        {
            return _permission.GetAllResourceRoles();
        }

        [HttpGet, AllowAnonymous]
        public virtual List<ResourceRoleInfo> GetOwnedResourceRoles(string userKey)
        {
            return _permission.GetOwnedResourceRoles(userKey);
        }


        /// <summary>
        /// 初始化权限资源
        /// </summary>
        [HttpGet, Resource(Description = "初始化权限资源")]
        public void InitResource()
        {
            _permission.InitResource();
        }

        [Resource(Description = "保存资源")]
        [HttpPost]
        public void SaveResource(PermissionResourceInfo saveDto)
        {
            _permissionStore.SaveResource(saveDto);
            _permissionStore.ReloadPemissionDatas();
        }

        [HttpPost, Resource(Description = "删除资源")]
        public void RemoveResource(List<string> ids)
        {
            ids.ForEach(id =>
            {
                _permissionStore.RemoveResource(id);
            });
            _permissionStore.ReloadPemissionDatas();
        }
        
        [HttpGet, Resource(Description = "查询资源树")]
        public List<PermissionResourceTreeInfo> GetAllResourceTreeInfo()
        {
            var allResources= _permissionStore.GetAllResource().Select(a => new PermissionResourceInfo { Code = a.GetResourceCode(), Id = a.GetKey(), Name = a.GetName(), ParentId = a.GetParentKey()}).ToList();
            return allResources.Where(a => !a.ParentId.HasValue()).Select(a => GetChildren(a, allResources)).ToList();
        }

        private PermissionResourceTreeInfo GetChildren(PermissionResourceInfo parent, List<PermissionResourceInfo> dtos)
        {
            return new PermissionResourceTreeInfo
            {
                Id = parent.Id,
                Code = parent.Code,
                Name = parent.Name,
                ParentId = parent.ParentId,
                Children = dtos.Where(a => a.ParentId == parent.Id).Select(a => GetChildren(a, dtos)).ToList()
            };
        }



        /// <summary>
        /// 用于权限的用户基本数据保存，请开发新接口以支持应用的用户保存逻辑
        /// </summary>
        /// <param name="user"></param>
        [HttpPost, Resource(Description = "保存用户")]
        public void SaveUser(PermissionUserInfo user)
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
            _permissionStore.SaveUser(user);
            _permissionStore.ReloadPemissionDatas();
        }
        [HttpPost, Resource(Description = "删除用户")]
        public void RemoveUser(string userKey)
        {
            _permissionStore.RemoveUser(userKey);
            _permissionStore.ReloadPemissionDatas();
        }

        /// <summary>
        /// 用于权限的角色基本数据保存，请开发新接口以支持应用的角色保存逻辑
        /// </summary>
        /// <param name="role"></param>
        [HttpPost, Resource(Description = "保存角色")]
        public void SaveRole(PermissionRoleInfo role)
        {
            _permissionStore.SaveRole(role);
            _permissionStore.ReloadPemissionDatas(); // todo ReloadPemissionDatas是否太重了
        }

        [HttpPost, Resource(Description = "删除角色")]
        public void RemoveRole(string roleKey)
        {
            _permissionStore.RemoveRole(roleKey);
            _permissionStore.ReloadPemissionDatas();
        }

        [HttpPost, Resource(Description = "用户授予角色")]
        public void SetUserRoles(PermissionUserRoleInfo dto)
        {
            _permissionStore.SetUserRoles(dto.UserKey, dto.RoleKeys);
            _permissionStore.ReloadPemissionDatas();
        }
        [HttpPost, Resource(Description = "角色授予资源")]
        public void SetRoleResources(PermissionRoleResourceInfo dto)
        {
            _permissionStore.SetRoleResources(dto.RoleKey, dto.ResourceKeys);
            _permissionStore.ReloadPemissionDatas();
        }

    }
}
