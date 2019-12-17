using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Snail.Core.Entity;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Snail.Core.Attributes;
using Snail.Core;

namespace Snail.DAL
{
    public class EFRepositoryWithCache<TEntity, TKey> : EFRepository<TEntity, TKey>, IRepositoryWithCache<TEntity, TKey> where TEntity : class,IEntityId<TKey>
    {
        private static string cachePrifex = "entityRepositoryCache";
        private IMemoryCache _memoryCache;
        private string _cacheKey;
        public EFRepositoryWithCache(DbContext db,IMemoryCache memoryCache):base(db)
        {
            _memoryCache = memoryCache;
            _cacheKey = $"{cachePrifex}_{typeof(TEntity).Name}";
        }
        public virtual List<TEntity> AllCache()
        {
            return InsureCache();
        }

        public virtual bool EnabledCache()
        {
            return Attribute.IsDefined(typeof(TEntity), typeof(EnableRepositoryCacheAttribute));
        }

        public virtual TEntity GetCacheEntity(TKey key)
        {
            var allEntities = InsureCache();
            return allEntities.FirstOrDefault(a => a.Id.Equals(key));
        }

        public virtual void ReloadCacheAll()
        {
            CheckEnabledCache();
            _memoryCache.Set(_cacheKey, _dbSet.AsNoTracking().ToList());
        }

        public virtual void ReloadCacheEntity(TKey key)
        {
            var cacheEntities = InsureCache();
            var entity = _dbSet.AsNoTracking().FirstOrDefault(a => a.Id.Equals(key));
            var old = cacheEntities.FirstOrDefault(a => a.Id.Equals(key));
            if (old==null)
            {
                cacheEntities.Add(entity);
            }
            else
            {
                old = entity;
            }
        }

        private List<TEntity> InsureCache()
        {
            CheckEnabledCache();
            if (!_memoryCache.TryGetValue(_cacheKey,out List<TEntity> value))
            {
                ReloadCacheAll();
            }
            return value;
        }

        private void CheckEnabledCache()
        {
            if (!EnabledCache())
            {
                throw new BusinessException($"类型{typeof(TEntity).Name}未启用repositoryCache");
            }
        }

    }
}
