using Snail.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Snail.Web.IServices
{
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
