using AutoMapper;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Snail.Cache;
using Snail.Core.Entity;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Snail.Web.Services
{
    /// <summary>
    /// service的上下文对象，封装所有service层用到的公共或是常用的属性，方法
    /// </summary>
    public class ServiceContext
    {
        public DbContext db;
        public IEntityCacheManager entityCacheManager;
        public IMapper mapper;
        public IApplicationContext applicationContext;
        public IMemoryCache memoryCache;
        public ICapPublisher publisher;
        public IServiceProvider serviceProvider;
        public ISnailCache cache;
        private HashSet<string> allCachedKeys = new HashSet<string>();
        private static object allCachedKeysLocker = new object();
        public ServiceContext(DbContext db, IMapper mapper, IApplicationContext applicationContext, IEntityCacheManager entityCacheManager, IMemoryCache memoryCache, ICapPublisher publisher, IServiceProvider serviceProvider,ISnailCache cache)
        {
            this.mapper = mapper;
            this.db = db;
            this.entityCacheManager = entityCacheManager;
            this.applicationContext = applicationContext;
            this.memoryCache = memoryCache;
            this.publisher = publisher;
            this.serviceProvider = serviceProvider;
            this.cache = cache;
        }

        /// <summary>
        /// 获取entity的缓存，用于TEntity会频繁修改，又需要自己控制cache刷新的情况（EntityCacheManager不适用的情况）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TEntityCache"></typeparam>
        /// <returns></returns>
        public List<TEntityCache> GetEntityCache<TEntity, TEntityCache>()
         where TEntity : class, IEntity
        {
            var cacheKey = GetEntityCacheKey<TEntity, TEntityCache>();
            lock (allCachedKeysLocker)
            {
                allCachedKeys.Add(cacheKey);
            }
            return cache.GetOrSet<List<TEntityCache>>(cacheKey, key =>
            {
                return mapper.ProjectTo<TEntityCache>(db.Set<TEntity>().AsNoTracking()).ToList();
            }, null);
        }

        public void ClearEntityCache<TEntity, TEntityCache>()
        {
            var cacheKey = GetEntityCacheKey<TEntity, TEntityCache>();
            lock (allCachedKeysLocker)
            {
                allCachedKeys.Remove(cacheKey);
            }
            cache.Remove(cacheKey);
        }

        public void ClearAllEntityCache()
        {
            allCachedKeys.ToList().ForEach(key =>
            {
                cache.Remove(key);
            });
            allCachedKeys.Clear();
        }
        public string GetEntityCacheKey<TEntity, TEntityCache>()
        {
            return $"cacheService_{typeof(TEntity).Name}_{typeof(TEntityCache).Name}";
        }

        public bool HasTenant(out string tenantId)
        {
            tenantId = applicationContext.GetCurrnetTenantId();
            return !string.IsNullOrEmpty(tenantId);
        }
    }
}
