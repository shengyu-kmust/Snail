using System.Collections.Generic;
using System.Security.Claims;

namespace Snail.Core.Permission
{
    /// <summary>
    /// 权限接口，这此接口是对外的，非对外的方法，不要写在接口里。
    /// </summary>
    public interface IPermission
    {
        #region 用于判断用户是否有资源权限的必要方法
        /// <summary>
        /// 通过访问的资源，获取资源的key。如obj可能为action，url
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string GetRequestResourceKey(object obj);
        /// <summary>
        /// 通过对象获取资源code
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string GetRequestResourceCode(object obj);

        /// <summary>
        /// 用户是否有资源的权限
        /// </summary>
        /// <param name="resourceKey">资源key</param>
        /// <param name="userKey">用户key</param>
        /// <returns></returns>
        bool HasPermission(string resourceKey, string userKey);
        /// <summary>
        /// 从ClaimsPrincipal获取用户信息
        /// </summary>
        /// <param name="claimsPrincipal">ClaimsPrincipal</param>
        /// <returns></returns>
        UserInfo GetUserInfo(ClaimsPrincipal claimsPrincipal);
        #endregion

        #region 登录、前端界面权限控制必要方法

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginDto">登录dto</param>
        /// <returns>如果登录成功，返回的结果；如果登录不成功，会抛出异常</returns>
        /// <remarks>
        /// 配置GetAllResourceRoles方法，可实现前端的权限控制
        /// </remarks>
        LoginResult Login(LoginDto loginDto);

        /// <summary>
        /// 获取所有的资源以及资源角色的对应关系信息
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 前端调用此接口，获取所有的资源及资源的角色，用于渲染界面权限控制
        /// </remarks>
        List<ResourceRoleInfo> GetAllResourceRoles();


        /// <summary>
        /// 通过userInfo生成Claims，Claims会用于生成token
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        List<Claim> GetClaims(IUserInfo userInfo);

        ///// <summary>
        ///// 获取登录token
        ///// </summary>
        ///// <param name="account"></param>
        ///// <param name="pwd"></param>
        ///// <returns></returns>
        //string GetLoginToken(string account, string pwd);

        ///// <summary>
        ///// 获取用户信息，用于给前端用户展示
        ///// </summary>
        ///// <param name="token"></param>
        //IUserInfo GetUserInfo(string token);

        #endregion

        #region 其它
        /// <summary>
        /// password的hash，可能加salt或是不加，hash的算法也可以由用户自己配置。
        /// 如果用户密码在存储时不做hash处理，则此方法返回pwd的明文即可
        /// 此方法用于两处
        /// 1、登录验证
        /// 2、修改、增加密码时
        /// </summary>
        /// <param name="pwd">用户输入的密码明文</param>
        /// <returns>密码明文的hash</returns>
        string HashPwd(string pwd);

        void InitResource();
        #endregion

    }
}
