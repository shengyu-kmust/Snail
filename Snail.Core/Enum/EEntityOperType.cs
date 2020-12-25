using Snail.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Snail.Core.Enum
{
    /// <summary>
    /// 实体的操作类型
    /// </summary>
    [EnumKeyValue]
    public enum EEntityOperType
    {
        /// <summary>
        /// 增加
        /// </summary>
        [Description("增加")]
        Add,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        Update,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete,
        /// <summary>
        /// 查询
        /// </summary>
        [Description("查询")]
        Query
    }
}
