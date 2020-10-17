using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core
{
    /// <summary>
    /// 审计字段
    /// </summary>
    /// <typeparam name="T">key类型</typeparam>
    public interface IAudit<T>
    {
        T Creater { get; set; }
        DateTime? CreateTime { get; set; }
        T Updater { get; set; }
        DateTime? UpdateTime { get; set; }
    }
}
