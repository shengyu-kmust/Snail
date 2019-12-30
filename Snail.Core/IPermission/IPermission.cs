using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Snail.Core.IPermission
{
    /// <summary>
    /// 权限接口
    /// </summary>
    public interface IPermission
    {
        
        /// <summary>
        /// 获取登录token
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        string GetLoginToken(string account, string pwd);

        /// <summary>
        /// 获取用户信息，用于给前端用户展示
        /// </summary>
        /// <param name="token"></param>
        IUserInfo GetUserInfo(string token);

        /// <summary>
        /// 获取所有的资源以及资源角色的对应关系信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<IResourceRoleInfo> GetAllResourceRoles();

        string GetUserKey(ClaimsPrincipal claimsPrincipal);


        string HashPwd(string pwd);

        bool HasPermission(string resourceKey,string userKey);

        /// <summary>
        /// 生成resource code
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string GetRequestResourceKey(object obj);

        /// <summary>
        /// 初始化所有的资源 
        /// </summary>
        void InitResource();
    }
}
