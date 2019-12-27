using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.IPermission
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public interface IUserInfo
    {
        /// <summary>
        /// 用户ID，系统的唯一键
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        string Account { get; set; }
        /// <summary>
        /// 用户角色ID
        /// </summary>
        List<string> RoleIds { get; set; }
    }
}
