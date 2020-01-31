using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Permission
{
    /// <summary>
    /// 用户信息，此信息将做为token信息返回给前端 
    /// </summary>
    public interface IUserInfo
    {
        /// <summary>
        /// 用户ID，系统的唯一键，对应IUser的Key
        /// </summary>
        string UserKey { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        string UserName { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        string Account { get; set; }
        /// <summary>
        /// 用户角色ID
        /// </summary>
        List<string> RoleKeys { get; set; }
        /// <summary>
        /// 用户角色名
        /// </summary>
        List<string> RoleNames { get; set; }
    }
}
