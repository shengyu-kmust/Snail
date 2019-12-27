using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.IPermission
{
    /// <summary>
    /// 权限接口
    /// </summary>
    public interface IPermission
    {
        #region 登录
        /// <summary>
        /// 获取登录token
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        string GetLoginToken(string account, string pwd);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="token"></param>
        IUserInfo GetUserInfo(string token);

        /// <summary>
        /// 获取所有资源和角色的对应关系信息
        /// </summary>
        /// <returns></returns>
        List<IResourceRole> GetAllResourceRoles();
        #endregion

        bool HasPermission(string resourceCode,string userId);

        /// <summary>
        /// 生成resource code
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string GenerateResourceCode(object obj);
    }
}
