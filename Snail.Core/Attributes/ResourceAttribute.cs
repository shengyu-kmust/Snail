using Snail.Core.Enum;
using System;

namespace Snail.Core.Attributes
{
    /// <summary>
    /// 定义权限资源的描述特性
    /// </summary>
    public class ResourceAttribute:Attribute
    {
        /// <summary>
        /// 资源code
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// 资源描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 资源类型
        /// </summary>
        public EResourceType ResourceType { get; set; }
    }
}
