﻿using System;

namespace Snail.Core.Entity
{
    public class DefaultBaseEntity : IEntityId<string>, IEntitySoftDelete, IEntityAudit<string>
    {
        public string Id { get; set; }
        public DateTime? CreateTime { get; set; } = DateTime.Now;
        public DateTime? UpdateTime { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public string Creater { get; set; }
        public string Updater { get; set; }
    }
}
