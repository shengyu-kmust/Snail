using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core
{
    /// <summary>
    /// 软删除
    /// </summary>
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
