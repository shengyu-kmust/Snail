using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Interface
{
    /// <summary>
    /// 提供应用上下文的信息访问，如用户id
    /// </summary>
    public interface IApplicationContext
    {
        string GetCurrentUserId();
        string GetCurrnetTenantId();
    }
}
