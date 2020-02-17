using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Snail.Core.Interface
{
    public interface ISelectorBuilder<TSource, TResult>
    {
        Expression<Func<TSource, TResult>> BuildSelector();
    }
}
