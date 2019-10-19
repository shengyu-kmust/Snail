using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Entity
{
    public interface IIdField<T>
    {
        T Id { get; set; }
    }
}
