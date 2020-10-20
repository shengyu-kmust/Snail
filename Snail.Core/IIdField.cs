using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core
{
    public interface IIdField<T>
    {
        T Id { get; set; }
    }
}
