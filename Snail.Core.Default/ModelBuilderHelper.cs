using Microsoft.EntityFrameworkCore;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snail.Core.Default
{
    /// <summary>
    /// ModelBuilderHelper
    /// </summary>
    public static class ModelBuilderHelper
    {
        /// <summary>
        /// 增加软删除和多租户的查询过滤器
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="applicationContext"></param>
        public static void AppContextConfigSoftDeletedAndTenantQueryFilter(ModelBuilder modelBuilder,IApplicationContext applicationContext)
        {
            var methodInfo = typeof(ModelBuilderHelper).GetMethod("SoftDeleteAndTenantQueryFilter");
            modelBuilder.Model.GetEntityTypes().Select(a=>a.ClrType).ToList().ForEach(entityType =>
            {
                var genericMethod = methodInfo.MakeGenericMethod(entityType);
                genericMethod.Invoke(null, new object[] { modelBuilder, applicationContext });
            });
        }

        /// <summary>
        /// 增加软删除和多租户的查询过滤器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="applicationContext"></param>
        public static void SoftDeleteAndTenantQueryFilter<T>(ModelBuilder modelBuilder, IApplicationContext applicationContext)
        where T : class
        {
            var isSoftDeleted = typeof(ISoftDelete).IsAssignableFrom(typeof(T));
            var isTenant = typeof(ITenant<string>).IsAssignableFrom(typeof(T));
            // 注意：目前ef的queryFilter只能对一个Entity执行一次，不能是多次，所以用下面的写法。
            if (isSoftDeleted && !isTenant)
            {
                modelBuilder.Entity<T>().HasQueryFilter(a => !((ISoftDelete)a).IsDeleted);
            }
            else if (isSoftDeleted && isTenant)
            {
                modelBuilder.Entity<T>().HasQueryFilter(a => !((ISoftDelete)a).IsDeleted
                &&
                (
                ((ITenant<string>)a).TenantId == null
                ||
                ((ITenant<string>)a).TenantId == ""
                ||
                ((ITenant<string>)a).TenantId == applicationContext.GetCurrnetTenantId()
                )
                );

            }
            else if (!isSoftDeleted && isTenant)
            {
                modelBuilder.Entity<T>().HasQueryFilter(a =>
                ((ITenant<string>)a).TenantId == null
                ||
                ((ITenant<string>)a).TenantId == ""
                ||
                ((ITenant<string>)a).TenantId == applicationContext.GetCurrnetTenantId()
                );
            }
            else
            {

            }
        }

    }
}
