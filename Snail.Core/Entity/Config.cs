using Snail.Core.Attributes;
using Snail.Core.Enum;

namespace Snail.Core.Entity
{
    [EnableEntityCache]
    public class Config : DefaultBaseEntity,ITenant<string>
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string ExtraInfo { get; set; }
        public int? Rank { get; set; }
        public EConfigOperType OperType { get; set; }
        public string TenantId { get; set; }
    }
}
