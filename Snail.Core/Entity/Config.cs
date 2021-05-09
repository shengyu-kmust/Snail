using Snail.Core.Attributes;
using Snail.Core.Enum;

namespace Snail.Core.Entity
{
    /// <summary>
    /// 配置字典，约定：存储数据库里的关联id为Config.Id，前端显示为Config.Value
    /// 
    /// </summary>
    [EnableEntityCache]
    public class Config : DefaultBaseEntity,ITenant<string>
    {
        /// <summary>
        /// 约定存储在其它关联表里的为Config.Id，特殊情况可用此字段
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 约定下拉的显示用value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 存储此配置描述信息，一般可与value同样
        /// </summary>
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string ExtraInfo { get; set; }
        public int? Rank { get; set; }
        public EConfigOperType OperType { get; set; }
        public string TenantId { get; set; }
    }
}
