using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Abstract.Entity
{
    /// <summary>
    /// 软删除
    /// </summary>
    public interface IEntitySoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
