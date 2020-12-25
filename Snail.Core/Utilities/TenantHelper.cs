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
    }
}
