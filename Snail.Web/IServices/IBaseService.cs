using Snail.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ApplicationCore.IServices
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        void Save<TSaveDto>(TSaveDto saveDto) where TSaveDto : IIdField<string>;
        void Remove(List<string> ids);
        IQueryable<TSource> QueryList<TSource>(Expression<Func<TSource, bool>> pred);
        IQueryable<TEntity> QueryList(Expression<Func<TEntity, bool>> pred);
    }
}
