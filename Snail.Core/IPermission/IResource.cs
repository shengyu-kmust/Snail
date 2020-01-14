using Snail.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.IPermission
{
    /// <summary>
    /// 资源（指所有要权限控制的资源，如接口,菜单）
    /// </summary>
    public interface IResource:IHasKeyAndName
    {
        /// <summary>
        /// 用于绑定到前端 
        /// </summary>
        /// <returns></returns>
        string GetResourceCode();
    }
}
