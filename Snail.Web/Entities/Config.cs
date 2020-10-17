using Snail.Core.Attributes;
using Snail.Core.Entity;
using Snail.Web.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snail.Web.Entities
{
    [EnableEntityCache]
    [Table("Config")]
    public class Config : DefaultBaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string ExtraInfo { get; set; }
        public int? Rank { get; set; }
        public EConfigOperType OperType { get; set; }
    }
}
