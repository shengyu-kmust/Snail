using Snail.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Snail.Core.Service
{
    /// <summary>
    /// 实体相关的service继承此接口，定义了实体的crud操作
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IBaseService<TEntity, TKey>
           where TEntity : class, IIdField<TKey>
    {
        void Save<TSaveDto>(TSaveDto saveDto) 
            where TSaveDto : class,IIdField<TKey>;
        void Remove(List<TKey> ids);
        IQueryable<TSource> QueryList<TSource>(Expression<Func<TSource, bool>> pred);
        IQueryable<TEntity> QueryList(Expression<Func<TEntity, bool>> pred);
    }
}
