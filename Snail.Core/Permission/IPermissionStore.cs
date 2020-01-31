using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Permission
{
    public interface IPermissionStore
    {
        #region 查询权限数据
        List<IUser> GetAllUser();
        List<IRole> GetAllRole();
        List<IUserRole> GetAllUserRole();
        List<IResource> GetAllResource();
        List<IRoleResource> GetAllRoleResource();
        #endregion

        #region 管理权限数据

        /// <summary>
        /// 初始化所有的资源 
        /// </summary>
        void InitResource();
        #endregion





    }
}
