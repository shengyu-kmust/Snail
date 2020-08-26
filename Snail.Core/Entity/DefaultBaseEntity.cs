using System;

namespace Snail.Core.Entity
{
    public class DefaultBaseEntity : IEntityId<string>, IEntitySoftDelete, IEntityAudit<string>
    {
        public string Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string Creater { get; set; }
        public string Updater { get; set; }
    }
}
