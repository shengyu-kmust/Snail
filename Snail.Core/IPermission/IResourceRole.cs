using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.IPermission
{
    public interface IResourceRole
    {
        /// <summary>
        /// 资源信息
        /// </summary>
        IResource Resource { get; set; }
        /// <summary>
        /// 资源所属的角色
        /// </summary>
        List<string> RoleIds { get; set; }
    }
}
