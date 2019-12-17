using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Interface
{
    /// <summary>
    /// 应用上下文
    /// </summary>
    public interface IApplicationContext
    {
        string GetCurrentUserId();
    }
}
