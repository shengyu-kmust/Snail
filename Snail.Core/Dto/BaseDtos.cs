using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Dto
{
    public class BaseDto<TKey> : IDtoId<TKey>
    {
        public TKey Id { get; set; }
    }

    public class BaseTenantDto<TKey> : BaseDto<TKey>, ITenant<TKey>
    {
        public TKey TenantId { get; set; }
    }

    public class BaseAuditDto<TKey> : IDtoId<TKey>, IAudit<TKey>
    {
        public TKey Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public TKey Creater { get; set; }
        public TKey Updater { get; set; }
    }


    public class BaseAuditTenantDto<TKey> : BaseAuditDto<TKey>, ITenant<TKey>
    {
        public TKey TenantId { get; set; }
    }


    public class BaseAuditSoftDeleteDto<TKey> : BaseAuditDto<TKey>, ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }

    public class BaseAuditSoftDeleteTenantDto<TKey> : BaseAuditSoftDeleteDto<TKey>, ITenant<TKey>
    {
        public TKey TenantId { get; set; }
    }
}
