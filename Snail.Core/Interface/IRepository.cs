using Snail.Core;
using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Snail.Core.Interface
{
    /// <summary>
    /// 数据仓库接口
    /// </summary>
    /// <remarks>
    /// 尽量简单
    /// </remarks>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity,TKey> where TEntity : IEntityId<TKey>
    {
        #region Add
        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);
        #endregion
        #region delete
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="key"></param>
        void Delete(TKey key);
        #endregion

        #region update
        /// <summary>
        /// 修改指定字段
        /// </summary>
        /// <param name="entity">修改的实体</param>
        /// <param name="changeProperties">要修改的字段</param>
        /// <returns></returns>
        void Update(TEntity entity, List<string> changeProperties);
        /// <summary>
        /// 全字段修改
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        #endregion

        #region 查
        List<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 通过主键查找单个实体
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        TEntity Find(TKey key);
        #endregion
    }
}
