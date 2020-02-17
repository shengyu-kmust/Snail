using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Snail.Core.Attributes;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snail.Core.Default
{
    public class EntityCacheManager : IEntityCacheManager, ICapSubscribe
    {
        public const string EntityCacheEventName = "EntityCacheEventName";
        private DbContext _db;
        private IMemoryCache _memoryCache;
        public EntityCacheManager(DbContext db, IMemoryCache memoryCache)
        {
            _db = db;
            _memoryCache = memoryCache;
        }

        public List<TEntity> Get<TEntity>() where TEntity : class
        {
            var key = GenerateEntityCacheKey<TEntity>();
            return _memoryCache.GetOrCreate<List<TEntity>>(key, a => LoadEntities<TEntity>());
        }

        private List<TEntity> LoadEntities<TEntity>() where TEntity : class
        {
            EnsureEnableCache<TEntity>();
            return _db.Set<TEntity>().AsNoTracking().ToList();

        }

        private void EnsureEnableCache<TEntity>()
        {
            if (!Attribute.IsDefined(typeof(TEntity), typeof(EnableEntityCacheAttribute)))
            {
                throw new BusinessException($"实体类{typeof(TEntity).Name}未启用缓存");
            }
        }

        private string GenerateEntityCacheKey<TEntity>()
        {
            return GenerateEntityCacheKey(typeof(TEntity).Name);
        }
        private string GenerateEntityCacheKey(string entityName)
        {
            return $"EntityCache_{entityName}".ToLower();
        }

        [CapSubscribe(EntityCacheManager.EntityCacheEventName)]
        public void ClearCache(EntityChangeEvent entityChangeEvent)
        {
            _memoryCache.Remove(GenerateEntityCacheKey(entityChangeEvent.EntityName));
        }


        public class EntityChangeEvent
        {
            public string EntityName { get; set; }
        }

       
    }
}
