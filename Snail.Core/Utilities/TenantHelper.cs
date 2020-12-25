using Snail.Core.Enum;
using System;
using System.Collections.Concurrent;

namespace Snail.Core.Utilities
{
    public static class TenantHelper
    {
        private static readonly ConcurrentDictionary<Type, bool> TypeIsTenantDic = new ConcurrentDictionary<Type, bool>();
        /// <summary>
        /// 是否为租户系统
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public static bool HasTenant<TEntity, TKey>()
        {
            return TypeIsTenantDic.GetOrAdd(typeof(TEntity), typeof(ITenant<TKey>).IsAssignableFrom(typeof(TEntity)));
        }

        /// <summary>
        /// 根据实体是否为租户系统，并返回实体的租户id
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="obj"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static bool HasTenant<TKey>(object obj,out TKey tenantId)
        {
            tenantId = default;
            var hasTenant = TypeIsTenantDic.GetOrAdd(obj.GetType(),obj is ITenant<TKey>);

            if (hasTenant)
            {
                tenantId = ((ITenant<TKey>)obj).TenantId;
            }
            return hasTenant;
        }

        public static void CheckEntityTenantOper<TEntity, TKey>(EEntityOperType operType, TEntity entity, TKey userId, TKey tenantId)
        where TEntity : class, IIdField<TKey>
        {
            // 跨租户实体操作限制
            if (tenantId != null && TenantHelper.HasTenant(entity, out TKey tenantIdTmp) && !tenantId.Equals(tenantIdTmp))
            {
                throw new InvalidOperationException($"不允许跨租户操作数据，操作类型:{operType}，表:{typeof(TEntity).Name}，实体id:{entity.Id}，操作人:{userId}，操作者租户:{tenantId}");
            }
        }
    }
}
