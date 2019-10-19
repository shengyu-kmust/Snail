using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Snail.DAL
{
    /// <summary>
    /// 查询条件转换接口
    /// </summary>
    /// <typeparam name="TDto">查询dto</typeparam>
    /// <typeparam name="TSource">查询的IQueryable对象</typeparam>
    public interface IPredicateConvert<TDto, TSource>
    {
        Expression<Func<TSource, bool>> GetExpression();
    }
}
