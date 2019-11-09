using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Interface
{
    /// <summary>
    /// 实体缓存
    /// </summary>
    public interface IEntityCacheManager
    {
        /// <summary>
        /// 从缓存里获取实体的所有列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TCacheItem"></typeparam>
        /// <returns></returns>
        List<TCacheItem> GetAll<TEntity, TCacheItem>() where TEntity : class;

        /// <summary>
        /// 从数据库里加载实体缓存
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TCacheItem"></typeparam>
        void ReLoadAll<TEntity, TCacheItem>() where TEntity : class;

        /// <summary>
        /// 从数据库里更新某个实体的缓存
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TCacheItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        void UpdateEntityCacheItem<TEntity, TCacheItem, TKey>(TKey key) where TEntity : class, IIdField<TKey> where TCacheItem : class, IIdField<TKey>;


    }

}
