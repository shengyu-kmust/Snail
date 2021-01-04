using Snail.Core.Attributes;
using System.ComponentModel;

namespace Snail.Core.Enum
{
    /// <summary>
    /// 字典配置的操作类型
    /// </summary>
    [EnumKeyValue]
    public enum EConfigOperType
    {
        /// <summary>
        /// 能进行全部的操作
        /// </summary>
        [Description("能进行全部的操作")]
        All,
        /// <summary>
        /// 只能修改值
        /// </summary>
        [Description("只能修改值")]
        OnlyChangeValue,
        /// <summary>
        /// 不能进行所有的操作
        /// </summary>
        [Description("不能进行所有的操作")]
        None,
    }
}
