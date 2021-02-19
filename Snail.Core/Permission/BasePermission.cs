using Snail.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Snail.Core.Permission
{
    /// <summary>
    /// 权限控制抽象基类，外部在实现权限控制时，如果继承此类，会简化实现的过程，也可以继承IPermission接口，自己实现 
    /// </summary>
    /// <remarks>
    /// todo 由于鉴权是频繁的操作，后期计划将鉴权方法里linq相关的操作用hash和缓存技术实现，进一步提高性能
    /// </remarks>
    public abstract class BasePermission : IPermission
    {
        protected IPermissionStore _permissionStore;
        protected abstract PermissionOptions PermissionOptions { set; get; }
        public BasePermission(IPermissionStore permissionStore)
        {
            _permissionStore = permissionStore;
        }

        #region 用于判断用户是否有资源权限的必要方法
        public virtual string GetRequestResourceKey(object obj)
        {
            var resourceKey = string.Empty;
            var resourceCode = GetRequestResourceCode(obj);
            if (!string.IsNullOrEmpty(resourceCode))
            {
                resourceKey = _permissionStore.GetAllResource().FirstOrDefault(a => a.GetResourceCode() == resourceCode)?.GetKey();
            }
            return resourceKey;
        }
        public abstract string GetRequestResourceCode(object obj);

        /// <summary>
        /// 权限判断
        /// 当资源未在资源表里时，不允许访问
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public virtual bool HasPermission(string resourceKey, string userKey)
        {
            var userRoleKeys = _permissionStore.GetAllUserRole().Where(a => a.GetUserKey() == userKey).Select(a => a.GetRoleKey());
            var resource = _permissionStore.GetAllResource().FirstOrDefault(a => a.GetKey() == resourceKey);

            //未纳入到资源表里的资源，如果进入到鉴权过程时，不允许访问。请将不需要做权限控制的资源设置成允许匿名访问，避免进入到鉴权流程
            if (resource == null)
            {
                return false;
            }
            var resourceRoleKeys = _permissionStore.GetAllRoleResource().Where(a => a.GetResourceKey() == resource.GetKey()).Select(a => a.GetRoleKey());
            return userRoleKeys.Intersect(resourceRoleKeys).Any();
        }
        public virtual UserInfo GetUserInfo(ClaimsPrincipal claimsPrincipal)
        {
            return new UserInfo
            {
                Account = claimsPrincipal.FindFirst(PermissionConstant.accountClaim)?.Value,
                RoleKeys = (claimsPrincipal.FindFirst(PermissionConstant.roleIdsClaim)?.Value ?? "").Split(',').ToList(),
                RoleNames = (claimsPrincipal.FindFirst(PermissionConstant.rolesNamesClaim)?.Value ?? "").Split(',').ToList(),
                UserKey = claimsPrincipal.FindFirst(PermissionConstant.userIdClaim)?.Value,
                UserName = claimsPrincipal.FindFirst(PermissionConstant.userNameClaim)?.Value,
                TenantId = claimsPrincipal.FindFirst(PermissionConstant.tenantIdClaim)?.Value,
            };
        }
        #endregion

        #region 登录、前端界面权限控制必要方法
        /// <summary>
        /// 登录，返回用户的基本信息和token
        /// </summary>
        /// <param name="loginDto">登录dto</param>
        /// <returns>用户的基本信息和token对象</returns>
        public virtual LoginResult Login(LoginDto loginDto)
        {
            IUser user;
            if (_permissionStore.HasTenant(out string tenantId))
            {
                if (string.IsNullOrEmpty(loginDto.TenantId))
                {
                    throw new BusinessException("多租户系统，必须传入租户id");
                }
                user = _permissionStore.GetAllUser().FirstOrDefault(a => a.GetAccount().Equals(loginDto.Account, StringComparison.OrdinalIgnoreCase) && ((ITenant<string>)a).TenantId==loginDto.TenantId);
            }
            else
            {
                user = _permissionStore.GetAllUser().FirstOrDefault(a => a.GetAccount().Equals(loginDto.Account, StringComparison.OrdinalIgnoreCase));
            }
            if (user != null && HashPwd(loginDto.Pwd).Equals(user.GetPassword(), StringComparison.OrdinalIgnoreCase))
            {
                var roleKeys = _permissionStore.GetAllUserRole().Where(a => a.GetUserKey() == user.GetKey()).Select(a => a.GetRoleKey()) ?? new List<string>();
                var roleNames = _permissionStore.GetAllRole().Where(a => roleKeys.Contains(a.GetKey())).Select(a => a.GetName()) ?? new List<string>();
                var userInfo = new UserInfo
                {
                    Account = user.GetAccount(),
                    RoleKeys = roleKeys.ToList(),
                    RoleNames = roleNames.ToList(),
                    UserKey = user.GetKey(),
                    UserName = user.GetName(),
                };
                if (user is ITenant<string> userTenant)
                {
                    userInfo.TenantId = userTenant.TenantId; // todo 让string为泛型
                }
                var claims = GetClaims(userInfo);
                var tokenStr = GenerateTokenStr(claims);
                return new LoginResult
                {
                    Token = tokenStr,
                    UserInfo = userInfo
                };
            }
            else
            {
                throw new BusinessException($"用户名或密码错误");
            }
        }
        public virtual List<ResourceRoleInfo> GetAllResourceRoles()
        {
            var result = new List<ResourceRoleInfo>();
            var allResource = _permissionStore.GetAllResource();
            var allRoleResource = _permissionStore.GetAllRoleResource();
            if (_permissionStore.HasTenant(out string tenantId))
            {
                var allRole = _permissionStore.GetAllRole().Where(a => ((ITenant<string>)a).TenantId == tenantId).Select(a => a.GetKey()).ToList();
                allRoleResource = allRoleResource.Where(a => allRole.Contains(a.GetRoleKey())).ToList();
            }
            allResource.ForEach(resource =>
            {
                var resourceRoleKeys = allRoleResource.Where(a => a.GetResourceKey() == resource.GetKey()).Select(a => a.GetRoleKey()).Distinct().ToList();
                result.Add(new ResourceRoleInfo
                {
                    ResourceCode = resource.GetResourceCode(),
                    ResourceKey = resource.GetKey(),
                    ResourceName = resource.GetName(),
                    RoleKeys = resourceRoleKeys
                });
            });
            return result;
        }
        public virtual List<ResourceRoleInfo> GetOwnedResourceRoles(string userKey)
        {
            var allResourceRoleInfo = GetAllResourceRoles();
            var userRoleKeys = _permissionStore.GetAllUserRole().Where(a => a.GetUserKey() == userKey).Select(a => a.GetRoleKey()).ToList();
            return allResourceRoleInfo.Where(a => userRoleKeys.Intersect(a.RoleKeys).Any()).ToList();
        }

        public virtual List<Claim> GetClaims(IUserInfo userInfo)
        {
            var claims= new List<Claim>
            {
                new Claim(PermissionConstant.userIdClaim,userInfo.UserKey),
                new Claim(PermissionConstant.userNameClaim,userInfo.UserName),
                new Claim(PermissionConstant.accountClaim,userInfo.Account),
                new Claim(PermissionConstant.roleIdsClaim,string.Join(",",userInfo.RoleKeys??new List<string>()) ),
                new Claim(PermissionConstant.rolesNamesClaim,string.Join(",",userInfo.RoleNames??new List<string>()) )
             };
            if (userInfo is ITenant<string> userInfoTenant)
            {
                claims.Add(new Claim(PermissionConstant.tenantIdClaim, userInfoTenant.TenantId));
            }
            return claims;
        }
        #endregion




        /// <summary>
        /// 默认的密码hash算法
        /// </summary>
        /// <param name="pwd">密码明文</param>
        /// <returns></returns>
        public virtual string HashPwd(string pwd)
        {
            return BitConverter.ToString(HashAlgorithm.Create(HashAlgorithmName.MD5.Name).ComputeHash(Encoding.UTF8.GetBytes(pwd))).Replace("-", "");
        }

        public abstract string GenerateTokenStr(List<Claim> claims);

        public abstract void InitResource();

    }
}
