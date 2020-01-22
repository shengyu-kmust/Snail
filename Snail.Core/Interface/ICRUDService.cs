using Snail.Core;
using Snail.Core.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Snail.Core.Interface
{
    /// <summary>
    /// 通用增、删、改、查处理
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <remarks>
    /// 默认提供EF的crud实现，即EFCRUDService，用此接口，需确认TEntity,TSource,TKey三个类型即可
    /// </remarks>
    public interface ICRUDService<TEntity,TSource,TKey> where TEntity : IEntityId<TKey> where TSource:class
    {
        IQueryable<TSource> GetQueryableSource();
        /// <summary>
        /// 分页查询 
        /// </summary>
        /// <typeparam name="TResult">分页查询的返回结果</typeparam>
        /// <typeparam name="TQueryDto">查询条件</typeparam>
        /// <param name="queryDto">查询dto</param>
        /// <returns>包含TResult的分页结果</returns>
        /// <remarks>
        /// 技巧：可以在此方法的实现里，支持通过id查询单个时，这样能确保单个查询和多个查询的逻辑和返回结果的一致性
        /// </remarks>
        IPageResult<TResult> QueryPage<TResult, TQueryDto>(TQueryDto queryDto) where TResult : class where TQueryDto : IPagination;
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TResult">查询的返回结果</typeparam>
        /// <typeparam name="TQueryDto">查询条件</typeparam>
        /// <param name="queryDto">查询dto</param>
        /// <returns>查询结果</returns>
        List<TResult> Query<TResult, TQueryDto>(TQueryDto queryDto) where TResult : class;
        /// <summary>
        /// 增加
        /// </summary>
        /// <typeparam name="TSaveDto">增加dto类型</typeparam>
        /// <param name="saveDto">增加dto</param>
        /// <returns>增加后的结果</returns>
        TEntity Add<TSaveDto>(TSaveDto saveDto) where TSaveDto : IIdField<TKey>;
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TSaveDto">更新dto类型</typeparam>
        /// <param name="saveDto">更新dto</param>
        /// <returns>更新后的结果</returns>
        TEntity Update<TSaveDto>(TSaveDto saveDto) where TSaveDto : IIdField<TKey>;
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">删除的TEntity的主键</param>
        void Delete(object id);
    }
}
