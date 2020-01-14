using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.IPermission
{
    public interface IHasKeyAndName
    {
        /// <summary>
        /// 一般为id，主键
        /// </summary>
        /// <returns></returns>
        string GetKey();
        /// <summary>
        /// 一般为描述
        /// </summary>
        /// <returns></returns>
        string GetName();
    }
}
