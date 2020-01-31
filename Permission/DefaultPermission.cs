using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core;
using Snail.Core.Attributes;
using Snail.Core.Permission;
using Snail.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace Snail.Permission
{
    public class DefaultPermission<TDbContext, TUser, TRole, TUserRole, TResource, TRoleResource> : IPermission
        where TUser : class, IUser, new()
        where TRole : class, IRole, new()
        where TUserRole : class, IUserRole, new()
        where TResource : class, IResource, new()
        where TRoleResource : class, IRoleResource, new()
        where TDbContext : DbContext
    {
        private static List<TUser> _users;
        private static List<TRole> _roles;
        private static List<TUserRole> _userRoles;
        private static List<TResource> _resources;
        private static List<TRoleResource> _roleResources;
        private IToken _token;
        private DbContext _dbContext;
        private PermissionOptions _permissionOptions;

        private IMemoryCache cache;
        public DefaultPermission(TDbContext dbContext, IToken token, IOptionsMonitor<PermissionOptions> permissionOptions)
        {
            _dbContext = dbContext;
            _token = token;
            _permissionOptions = permissionOptions?.CurrentValue ?? new PermissionOptions();
            LoadDatas();
        }

        /// <summary>
        /// 已经适配类型为AuthorizationFilterContext或是ControllerActionDescriptor的obj获取key
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetRequestResourceKey(object obj)
        {
            var resourceKey = string.Empty;
            var resourceCode = string.Empty;
            if (obj is AuthorizationFilterContext authorizationFilterContext)
            {
                if (authorizationFilterContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                {
                    resourceCode = GetResourceCode(controllerActionDescriptor.ControllerName, controllerActionDescriptor.ActionName);
                }
            }
            if (obj is ControllerActionDescriptor controllerActionDescriptor1)
            {
                resourceCode = GetResourceCode(controllerActionDescriptor1.ControllerName, controllerActionDescriptor1.ActionName);
            }
            if (resourceCode.HasValue())
            {
                resourceKey = _resources.FirstOrDefault(a => a.GetResourceCode() == resourceCode)?.GetKey();
            }
            return resourceKey;
        }



        public IEnumerable<IResourceRoleInfo> GetAllResourceRoles()
        {
            return _roleResources.GroupBy(a => a.GetResourceKey()).Select(a => new ResourceRoleInfo
            {
                ResourceKey = a.Key,
                RoleKeys = a.Select(i => i.GetRoleKey()).ToList()
            });
        }

        public string GetLoginToken(string account, string pwd)
        {
            var user = _users.FirstOrDefault(a => a.GetAccount() == account);
            if (user != null && HashPwd(pwd).Equals(user.GetPassword()))
            {
                var roleKeys = _userRoles.Where(a => a.GetUserKey() == user.GetKey()).Select(a => a.GetRoleKey()) ?? new List<string>();
                var roleNames = _roles.Where(a => roleKeys.Contains(a.GetKey())).Select(a => a.GetName()) ?? new List<string>();
                var claims = new List<Claim>
                {
                    new Claim(PermissionConstant.userIdClaim,user.GetKey()),
                    new Claim(PermissionConstant.userNameClaim,user.GetName()),
                    new Claim(PermissionConstant.accountClaim,user.GetAccount()),
                    new Claim(PermissionConstant.roleIdsClaim,string.Join(",", roleKeys)),
                    new Claim(PermissionConstant.userNameClaim,string.Join(",", roleNames)),
                };

                return _token.ResolveToken(claims, DateTime.Now.Add(PermissionConstant.tokenExpire));
            }
            else
            {
                throw new BusinessException($"用户名或密码错误");
            }
        }


        public IUserInfo GetUserInfo(string token)
        {
            var claims = _token.ResolveFromToken(token);
            return new UserInfo
            {
                UserKey = claims.FirstOrDefault(a => a.Type == PermissionConstant.userIdClaim)?.Value,
                Account = claims.FirstOrDefault(a => a.Type == PermissionConstant.accountClaim)?.Value,
                UserName = claims.FirstOrDefault(a => a.Type == PermissionConstant.userNameClaim)?.Value,
                RoleKeys = claims.FirstOrDefault(a => a.Type == PermissionConstant.roleIdsClaim)?.Value?.Split(',').ToList(),
                RoleNames = claims.FirstOrDefault(a => a.Type == PermissionConstant.rolesNameClaim)?.Value?.Split(',').ToList(),
            };
        }

        public bool HasPermission(string resourceKey, string userKey)
        {
            var userRoleKeys = _userRoles.Where(a => a.GetUserKey() == userKey).Select(a => a.GetRoleKey());
            var resource = _resources.FirstOrDefault(a => a.GetKey() == resourceKey);
            var resourceRoleKeys = _roleResources.Where(a => a.GetResourceKey() == resource.GetKey()).Select(a => a.GetRoleKey());
            return userRoleKeys.Intersect(resourceRoleKeys).Any();
        }

        #region 
        private void LoadDatas()
        {
            // todo 可优化
            lock (Locker.GetLocker("DefaultPermission_LoadDatas"))
            {
                _users = _dbContext.Set<TUser>().AsNoTracking().ToList();
                _roles = _dbContext.Set<TRole>().AsNoTracking().ToList();
                _userRoles = _dbContext.Set<TUserRole>().AsNoTracking().ToList();
                _resources = _dbContext.Set<TResource>().AsNoTracking().ToList();
                _roleResources = _dbContext.Set<TRoleResource>().AsNoTracking().ToList();
            }
        }

        public string GetUserKey(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.FirstOrDefault(a => a.Type == PermissionConstant.userIdClaim).Value;
        }

        public string HashPwd(string pwd)
        {
            return HashHelper.Md5($"{pwd}{_permissionOptions.PasswordSalt}");
        }

        public void InitResource()
        {
            var resources = new List<Resource>();
            _permissionOptions.ResourceAssemblies?.ToList().ForEach(assembly =>
            {
                assembly.GetTypes().Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToList().ForEach(controller =>
                 {
                     var controllerIsAdded = false;
                     var parentId = IdGenerator.Generate<string>();
                     controller.GetMethods().ToList().ForEach(method =>
                     {
                         if (method.IsDefined(typeof(AuthorizeAttribute), true))
                         {
                             if (!controllerIsAdded)
                             {
                                 resources.Add(new Resource
                                 {
                                     Id = parentId,
                                     Code = GetResourceCode(controller.Name, method.Name),
                                     CreateTime = DateTime.Now,
                                     IsDeleted = false,
                                     Name = method.IsDefined(typeof(PermissionAttribute)) ? ((PermissionAttribute)method.GetCustomAttribute(typeof(PermissionAttribute))).Description : GetResourceCode(controller.Name, method.Name)
                                 });
                                 controllerIsAdded = true;
                             }
                             resources.Add(new Resource
                             {
                                 Id = IdGenerator.Generate<string>(),
                                 Code = GetResourceCode(controller.Name, method.Name),
                                 CreateTime = DateTime.Now,
                                 IsDeleted = false,
                                 ParentId=parentId,
                                 Name = method.IsDefined(typeof(PermissionAttribute)) ? ((PermissionAttribute)method.GetCustomAttribute(typeof(PermissionAttribute))).Description : GetResourceCode(controller.Name, method.Name)
                             });
                         }
                     });
                 });
            });
            _dbContext.Add(resources);
            _dbContext.SaveChanges();
        }


        #endregion

        private string GetResourceCode(string className, string methodName)
        {
            return $"{className.Replace("Controller", "")}_{methodName}";
        }
    }
}
