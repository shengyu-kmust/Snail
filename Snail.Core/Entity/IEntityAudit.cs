using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Entity
{
    /// <summary>
    /// 审计字段
    /// </summary>
    /// <typeparam name="T">key类型</typeparam>
    public interface IEntityAudit<T>
    {
        T CreaterId { get; set; }
        DateTime CreateTime { get; set; }
        T UpdaterId { get; set; }
        DateTime UpdateTime { get; set; }
    }
}
