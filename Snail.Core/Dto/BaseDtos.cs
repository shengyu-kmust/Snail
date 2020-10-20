using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Dto
{
    public class BaseAuditDto<T> : IDtoId<T>, IAudit<T>
    {
        public T Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public T Creater { get; set; }
        public T Updater { get; set; }
    }


    public class BaseAuditTenantDto<T> : BaseAuditDto<T>, ITenant<T>
    {
        public T TenantId { get; set; }
    }


    public class BaseAuditSoftDeleteDto<T> : BaseAuditDto<T>, ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }

    public class BaseAuditSoftDeleteTenantDto<T> : BaseAuditSoftDeleteDto<T>, ITenant<T>
    {
        public T TenantId { get; set; }
    }
}
