using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Snail.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Snail.Core.Permission
{
    /// <summary>
    /// 权限控制抽象基类，外部在实现权限控制时，如果继承此类，会简化实现的过程，也可以继承IPermission接口，自己实现 
    /// </summary>
    public abstract class BasePermission : IPermission
    {
        private IPermissionStore _permissionStore;
        private PermissionOptions _permissionOptions;
        public BasePermission(IPermissionStore permissionStore, IOptionsMonitor<PermissionOptions> permissionOptions)
        {
            _permissionStore = permissionStore;
            _permissionOptions = permissionOptions.CurrentValue??new PermissionOptions();
        }
        public string GetLoginToken(string account, string pwd)
        {
            var user = _permissionStore.GetAllUser().FirstOrDefault(a => a.GetAccount() == account);
            if (user != null && HashPwd(pwd).Equals(user.GetPassword()))
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
                var claimsPrincipal=GetClaimsPrincipal(userInfo);
                //var claims = new List<Claim>
                //{
                //    new Claim(PermissionConstant.userIdClaim,user.GetKey()),
                //    new Claim(PermissionConstant.userNameClaim,user.GetName()),
                //    new Claim(PermissionConstant.accountClaim,user.GetAccount()),
                //    new Claim(PermissionConstant.roleIdsClaim,string.Join(",", roleKeys)),
                //    new Claim(PermissionConstant.userNameClaim,string.Join(",", roleNames)),
                //};

                //var key = new RsaSecurityKey(RSAHelper.GetRSAParametersFromFromPrivatePem(_permissionOptions.RsaPrivateKey));
                //var jwtSecurityToken = new JwtSecurityToken(null, null, claims, null, expireTime, new SigningCredentials(key, SecurityAlgorithms.RsaSha256));
                //var tokenStr = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                //_cache.Set(tokenStr, claims, expireTime);

                return GenerateTokenStr(claimsPrincipal);
                //return _token.ResolveToken(claims, DateTime.Now.Add(PermissionConstant.tokenExpire));
            }
            else
            {
                throw new BusinessException($"用户名或密码错误");
            }
        }



        public IEnumerable<IResourceRoleInfo> GetAllResourceRoles()
        {
            throw new NotImplementedException();
        }

       

        public string GetRequestResourceKey(object obj)
        {
            throw new NotImplementedException();
        }

        public IUserInfo GetUserInfo(string token)
        {
            throw new NotImplementedException();
        }

        public string GetUserKey(ClaimsPrincipal claimsPrincipal)
        {
            throw new NotImplementedException();
        }

        public string HashPwd(string pwd)
        {
            throw new NotImplementedException();
        }

        public bool HasPermission(string resourceKey, string userKey)
        {
            throw new NotImplementedException();
        }

        public void InitResource()
        {
            throw new NotImplementedException();
        }

        public abstract IUserInfo GetUserInfo(ClaimsPrincipal claimsPrincipal);

        public abstract ClaimsPrincipal GetClaimsPrincipal(IUserInfo userInfo);
        public abstract string GenerateTokenStr(ClaimsPrincipal claimsPrincipal);

        public LoginResult Login(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        List<IResourceRoleInfo> IPermission.GetAllResourceRoles()
        {
            throw new NotImplementedException();
        }
    }
}
