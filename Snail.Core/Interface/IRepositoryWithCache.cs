using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Interface
{
    /// <summary>
    /// 实体缓存 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepositoryWithCache<TEntity,TKey>  :IRepository<TEntity,TKey> where TEntity:class,IEntityId<TKey>
    {
        bool EnabledCache();
        TEntity GetCacheEntity(TKey key);
        List<TEntity> AllCache();
        void ReloadCacheEntity(TKey key);
        void ReloadCacheAll();
    }
}
