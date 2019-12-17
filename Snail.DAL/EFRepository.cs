using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Snail.Common;
using Snail.Core;
using Snail.Core.Entity;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Snail.DAL
{

    /// <summary>
    /// 基于entityframework的数据仓储模式
    /// <remarks>由于entityframework支持多种数据库操作，因此数据仓储只实现一个EFRepository，切换数据库时只要切换DbContext的不同实现即可，代码不需要做更改</remarks>
    /// </summary>
    public class EFRepository<TEntity,TKey> : IRepository<TEntity,TKey> where TEntity : class, IEntityId<TKey>
    {
        protected readonly DbContext _dbContext;
        protected readonly IApplicationContext _applicationContext;
        protected DbSet<TEntity> _dbSet;
       
        public EFRepository(DbContext dbContext, IApplicationContext applicationContext)
        {
            _dbContext = dbContext;
            _applicationContext = applicationContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        #region add
        public virtual void Add(TEntity entity)
        {
            var userId = (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromString(_applicationContext.GetCurrentUserId());
            if (entity.Id.Equals(default) || entity.Id==null)
            {
                entity.Id = IdGenerator.Generate<TKey>();
            }
            if (entity is IEntityAudit<TKey> entityAudit)
            {
                entityAudit.UpdaterId = userId;
                entityAudit.UpdateTime = DateTime.Now;
                entityAudit.CreaterId = userId;
                entityAudit.

                entityAudit.UpdaterId = _applicationContext.GetCurrentUserId();
            }


            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }
        #endregion
        #region delete
        public virtual void Delete(TKey key)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region update
        public virtual void Update(TEntity entity, List<string> changeProperties)
        {
            var entityEntry = _dbContext.Entry(entity);
            foreach (var property in changeProperties)
            {
                entityEntry.Property(property).IsModified = true;
            }
            _dbContext.SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            _dbContext.Update(entity);
            _dbContext.SaveChanges();
        }
        #endregion



        #region 查找
        public virtual List<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(predicate);
        }
        public virtual TEntity Find(TKey key)
        {
            return _dbSet.Find(key);
        }


        #endregion

      
    }
}
