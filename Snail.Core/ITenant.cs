using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core
{
    public interface ITenant<T>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        T TenantId { get; set; }
    }
}
