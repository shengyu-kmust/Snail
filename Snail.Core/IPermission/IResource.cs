using Snail.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.IPermission
{
    /// <summary>
    /// 资源（指所有要权限控制的资源，如接口,菜单）
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// 资源唯一code
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        EResourceType ResourceType { get; set; }

    }
}
