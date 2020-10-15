using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Entity
{
    public interface ITenant<T>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        T TenantId { get; set; }
    }
}
