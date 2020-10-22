using Snail.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Permission
{
    /// <summary>
    /// 资源（指所有要权限控制的资源，如接口,菜单）
    /// </summary>
    public interface IResource:IHasKeyAndName
    {
        /// <summary>
        /// 用于绑定到前端，前端在做权限和界面元素的绑定时，一般不会用id（id可读性差）和name（name可能会改变），一般以code做约定
        /// </summary>
        /// <returns></returns>
        string GetResourceCode();
        /// <summary>
        /// 获取父级
        /// </summary>
        /// <returns></returns>
        string GetParentKey();
        /// <summary>
        /// 设置父级
        /// </summary>
        /// <returns></returns>
        void SetParentKey(string parentKey);
    }
}
