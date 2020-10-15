using System;

namespace Snail.Core.Entity
{
    public class DefaultBaseEntityAudit : IEntityId<string>, IEntityAudit<string>
    {
        public string Id { get; set; }
        public DateTime? CreateTime { get; set; } = DateTime.Now;
        public DateTime? UpdateTime { get; set; } = DateTime.Now;
        public string Creater { get; set; }
        public string Updater { get; set; }
    }
}
