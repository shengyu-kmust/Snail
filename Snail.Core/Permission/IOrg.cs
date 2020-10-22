using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Permission
{
    public interface IOrg : IHasKeyAndName
    {
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
