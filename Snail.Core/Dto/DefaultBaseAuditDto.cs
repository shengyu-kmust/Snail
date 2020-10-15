using Snail.Core.Entity;
using System;

namespace Snail.Core.Dto
{
    public class DefaultBaseAuditDto : DefaultBaseDto, IEntityAudit<string>, IEntitySoftDelete
    {
        public string Creater {get;set; }
        public DateTime? CreateTime {get;set; }
        public string Updater {get;set; }
        public DateTime? UpdateTime {get;set; }
        public bool IsDeleted {get;set; }

    }
}
