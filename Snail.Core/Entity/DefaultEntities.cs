using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Entity
{
    public class DefaultBaseEntity : BaseAuditSoftDeleteEntity<string>
    {
    }

    public class DefaultBaseEntityWithTenant : BaseAuditSoftDeleteTenantEntity<string>
    {
    }

}
