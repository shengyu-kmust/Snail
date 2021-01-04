using Microsoft.EntityFrameworkCore;
using Snail.Core.Service;
using Snail.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Snail.Core.Default.Service
{
    public abstract class BaseService<TEntity,TKey> : ServiceContextBaseService, IBaseService<TEntity,TKey>
           where TEntity : class, IIdField<TKey>
    {
        protected BaseService(ServiceContext serviceContext) : base(serviceContext)
        {
        }

        public virtual IQueryable<TSource> GetQueryable<TSource>()
        {
            if (typeof(TSource) == typeof(TEntity))
            {
                return (IQueryable<TSource>)db.Set<TEntity>().AsNoTracking();
            }
            throw new NotSupportedException();
        }

        public virtual IQueryable<TSource> QueryList<TSource>(Expression<Func<TSource, bool>> pred)
        {
            return GetQueryable<TSource>().Where(pred);
        }
        public virtual IQueryable<TEntity> QueryList(Expression<Func<TEntity, bool>> pred)
        {
            return GetQueryable<TEntity>().Where(pred);
        }

        public virtual void Remove(List<TKey> ids)
        {
            db.Set<TEntity>().RemoveByIds(ids, GetCurrentUserId());
            db.SaveChanges();
        }

        /// <summary>
        /// 增加或修改
        /// </summary>
        /// <typeparam name="TSaveDto"></typeparam>
        /// <param name="saveDto"></param>
        public virtual void Save<TSaveDto>(TSaveDto saveDto) 
            where TSaveDto : class, IIdField<TKey>
        {
            db.Set<TEntity>().AddOrUpdate(saveDto, mapper, GetCurrentUserId());
            db.SaveChanges();
        }
        private TKey GetCurrentUserId()
        {
            return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromString(applicationContext.GetCurrentUserId());
        }
    }
}
