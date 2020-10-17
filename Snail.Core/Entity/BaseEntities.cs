using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Entity
{
    public class BaseAuditEntity<T> : IEntityId<T>, IAudit<T>
    {
        public T Id { get; set; }
        public DateTime? CreateTime { get; set; } = DateTime.Now;
        public DateTime? UpdateTime { get; set; } = DateTime.Now;
        public T Creater { get; set; }
        public T Updater { get; set; }
    }
    public class BaseAuditTenantEntity<T> : BaseAuditEntity<T>, ITenant<T>
    {
        public T TenantId { get;set;}
    }


    public class BaseAuditSoftDeleteEntity<T> : BaseAuditEntity<T>,ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }

    public class BaseAuditSoftDeleteTenantEntity<T> : BaseAuditSoftDeleteEntity<T>, ITenant<T>
    {
        public T TenantId { get;set;}
    }
}
