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
    public abstract class BasePermission : IPermission
    {
        protected IPermissionStore _permissionStore;
        protected abstract PermissionOptions PermissionOptions {set;get;}
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
        public virtual bool HasPermission(string resourceKey, string userKey)
        {
            var userRoleKeys = _permissionStore.GetAllUserRole().Where(a => a.GetUserKey() == userKey).Select(a => a.GetRoleKey());
            var resource = _permissionStore.GetAllResource().FirstOrDefault(a => a.GetKey() == resourceKey);
            
            //未纳入到资源表里的资源，不允许访问
            if (resource==null)
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
            };
        }
        #endregion

        #region 登录、前端界面权限控制必要方法
        public virtual LoginResult Login(LoginDto loginDto)
        {
            var user = _permissionStore.GetAllUser().FirstOrDefault(a => a.GetAccount() == loginDto.Account);
            if (user != null && HashPwd(loginDto.Pwd).Equals(user.GetPassword()))
            {
                var roleKeys = _permissionStore.GetAllUserRole().Where(a => a.GetUserKey() == user.GetKey()).Select(a => a.GetRoleKey()) ?? new List<string>();
                var roleNames = _permissionStore.GetAllRole().Where(a => roleKeys.Contains(a.GetKey())).Select(a => a.GetName()) ?? new List<string>();
                var userInfo = new UserInfo
                {
                    Account = user.GetAccount(),
                    RoleKeys = roleKeys.ToList(),
                    RoleNames = roleNames.ToList(),
                    UserKey = user.GetKey(),
                    UserName = user.GetName()
                };
                var claims = GetClaims(userInfo);
                var tokenStr= GenerateTokenStr(claims);
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
            var allRole = _permissionStore.GetAllRole();
            var allRoleResource = _permissionStore.GetAllRoleResource();
            allResource.ForEach(resource =>
            {
                var resourceRoleKeys = allRoleResource.Where(a => a.GetResourceKey() == resource.GetKey()).Select(a => a.GetRoleKey()).Distinct().ToList();
                result.Add(new ResourceRoleInfo
                {
                    ResourceCode=resource.GetResourceCode(),
                    ResourceKey=resource.GetKey(),
                    ResourceName=resource.GetName(),
                    RoleKeys= resourceRoleKeys
                });
            });
            return result;
        }
        public virtual List<Claim> GetClaims(IUserInfo userInfo)
        {
            return new List<Claim>
            {
                new Claim(PermissionConstant.userIdClaim,userInfo.UserKey),
                new Claim(PermissionConstant.userNameClaim,userInfo.UserName),
                new Claim(PermissionConstant.accountClaim,userInfo.Account),
                new Claim(PermissionConstant.roleIdsClaim,string.Join(",",userInfo.RoleKeys??new List<string>()) ),
                new Claim(PermissionConstant.rolesNamesClaim,string.Join(",",userInfo.RoleNames??new List<string>()) ),
            };
        }
        #endregion




        /// <summary>
        /// 默认的密码hash算法
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public virtual string HashPwd(string pwd)
        {
            return BitConverter.ToString(HashAlgorithm.Create(HashAlgorithmName.MD5.Name).ComputeHash(Encoding.UTF8.GetBytes(pwd))).Replace("-", "");
        }
   
        public abstract string GenerateTokenStr(List<Claim> claims);

        public abstract void InitResource();
        
    }
}
