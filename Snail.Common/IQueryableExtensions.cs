using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Snail.Common
{
    public static class IQueryableExtensions
    {
        // snail.core里也有这个类，但两者类所依赖的东西是不一样的，此类不依赖任务其它的框架
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query,bool condition,Expression<Func<T,bool>> predicate)
        {
            if (condition)
            {
                return query.Where(predicate);
            }
            else
            {
                return query;
            }
        }

        public static IQueryable<T> WhereIfElse<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> truePredicate, Expression<Func<T, bool>> falsePredicate)
        {
            if (condition)
            {
                return query.Where(truePredicate);
            }
            else
            {
                return query.Where(falsePredicate);
            }
        }
    }
}
