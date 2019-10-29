using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Core
{
    public interface IUserRoleManager<TUser,TRole,TUserRole>
    {
        void AddUser(TUser user);
        void AddRole(TRole role);
        /// <summary>
        /// 取笛卡尔集后再添加
        /// </summary>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        void AddUserRoles(List<TUser> users, List<TRole> roles);
        /// <summary>
        /// 以id为依据删除，从TUserRole里获取key
        /// </summary>
        /// <param name="userRoles"></param>
        void DeleteUserRoles(List<TUserRole> userRoles);
        /// <summary>
        /// 假删除用户
        /// </summary>
        /// <param name="user"></param>
        void DeleteUsers(List<TUser> users);

        /// <summary>
        /// 真删除角色
        /// </summary>
        /// <param name="roles"></param>
        void DeleteRoles(List<TRole> roles);

    }
}
