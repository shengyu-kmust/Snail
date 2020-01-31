using AutoMapper;
using EasyCaching.Core;
using Microsoft.EntityFrameworkCore;
using Snail.Core.Entity;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Snail.Core
{
    #region 不再用
    ///// <summary>
    ///// 约定：一个TEntity只能对应一个TCache
    ///// </summary>
    //public class EFEntityCacheManager : IEntityCacheManager
    //{
    //    private readonly IEasyCachingProviderFactory _factory;
    //    private IMapper _mapper;
    //    private IEasyCachingProvider _cacheProvider;
    //    private DbContext _db;
    //    public const string EntityCacheName = "EntityMemoryCache";
    //    private TimeSpan DefaultCacheTimeSpan = new TimeSpan(1, 0, 0, 0);
    //    public EFEntityCacheManager(IMapper mapper, IEasyCachingProviderFactory factory, DbContext db)
    //    {
    //        _factory = factory;
    //        _mapper = mapper;
    //        _db = db;
    //        _cacheProvider = factory.GetCachingProvider(EntityCacheName);
    //    }

    //    public List<TCacheItem> GetAll<TEntity, TCacheItem>() where TEntity : class
    //    {
    //        var cacheKey = EntityCacheKey<TEntity, TCacheItem>();
    //        return _cacheProvider.Get<List<TCacheItem>>(cacheKey, GetCacheItemsFromDb<TEntity, TCacheItem>, DefaultCacheTimeSpan).Value;
    //    }
    //    public void ReLoadAll<TEntity, TCacheItem>() where TEntity : class
    //    {
    //        _cacheProvider.Set(EntityCacheKey<TEntity, TCacheItem>(), GetCacheItemsFromDb<TEntity, TCacheItem>(), DefaultCacheTimeSpan);
    //    }
    //    public void UpdateEntityCacheItem<TEntity, TCacheItem, TKey>(TKey key) where TEntity : class, IIdField<TKey> where TCacheItem : class, IIdField<TKey>
    //    {
    //        var entity = _db.Set<TEntity>().AsNoTracking().FirstOrDefault(CreateEqualityExpressionForId<TEntity, TKey>(key));
    //        var cacheKey = EntityCacheKey<TEntity, TCacheItem>();
    //        var cacheItems = _cacheProvider.Get<List<TCacheItem>>(cacheKey).Value;
    //        if (cacheItems == null)
    //        {
    //            throw new Exception($"{cacheKey}缓存不存在");
    //        }
    //        var oldCacheItem = cacheItems.FirstOrDefault(CreateEqualityExpressionForId<TCacheItem, TKey>(key).Compile());
    //        if (entity != null)
    //        {
    //            // save
    //            var newCacheItem = _mapper.Map<TCacheItem>(entity);
    //            if (oldCacheItem == default(TCacheItem) || oldCacheItem == null)
    //            {
    //                cacheItems.Add(newCacheItem);//增加
    //            }
    //            else
    //            {
    //                _mapper.Map(newCacheItem, oldCacheItem);//更新
    //            }
    //        }
    //        else
    //        {
    //            //delete 
    //            if (oldCacheItem != default(TCacheItem))
    //            {
    //                cacheItems.Remove(oldCacheItem);
    //            }
    //        }

    //    }

    //    private List<TCacheItem> GetCacheItemsFromDb<TEntity, TCacheItem>() where TEntity : class
    //    {
    //        var allEntities = _db.Set<TEntity>().AsNoTracking().ToList();
    //        var allCacheItem = _mapper.Map<List<TCacheItem>>(allEntities);
    //        return allCacheItem;
    //    }

    //    private string EntityCacheKey<TEntity, TCacheItem>()
    //    {
    //        return $"EntityCache_{typeof(TEntity).Name}_{typeof(TEntity).Name}";
    //    }

    //    private Expression<Func<T, bool>> CreateEqualityExpressionForId<T, TKey>(TKey id) where T : class, IIdField<TKey>
    //    {
    //        var lambdaParam = Expression.Parameter(typeof(T));

    //        var lambdaBody = Expression.Equal(
    //            Expression.PropertyOrField(lambdaParam, "Id"),
    //            Expression.Constant(id, typeof(TKey))
    //            );

    //        return Expression.Lambda<Func<T, bool>>(lambdaBody, lambdaParam);
    //    }


    //} 
    #endregion
}
