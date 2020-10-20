using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Dto
{
    /// <summary>
    /// 包含主键的dto
    /// </summary>
    /// <typeparam name="T">主键的类型</typeparam>
    public interface IDtoId<T> : IIdField<T>, IDto
    {
    }
}
