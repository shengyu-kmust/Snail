using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Snail.Core.Permission;
using Snail.Permission.Entity;
using System;
namespace Snail.Permission
{
    /// <summary>
    /// 实现权限的方式有如下两种
    /// 1、dbContext继承PermissionDatabaseContext，并调用AddDefaultPermission
    /// 2、实现IPermission和IPermissionStore接口并注册，调用AddPermissionCore
    /// </summary>
    public static class PermissionServiceCollectionExtensions
    {
        

        /// <summary>
        /// 用默认的User, Role, UserRole, Resource, RoleResource表实现权限,即TDbContext已经有默认的这几张表，TDbContext可以通过继承PermissionDatabaseContext来简化实现过程
        /// </summary>
        /// <typeparam name="TDbContext">权限的表是在哪个dbcontext</typeparam>
        /// <param name="services"></param>
        /// <param name="action"></param>
        public static void AddDefaultPermission<TDbContext>(this IServiceCollection services, Action<PermissionOptions> action) where TDbContext:DbContext
        {
            if (!typeof(PermissionDatabaseContext).IsAssignableFrom(typeof(TDbContext)))
            {
                throw new Exception($"{typeof(TDbContext).Name}未继承{typeof(PermissionDatabaseContext).Name}");
            }
            services.TryAddScoped<DbContext, TDbContext>();
            services.TryAddScoped<IPermission, DefaultPermission>();
            services.TryAddScoped<IPermissionStore, DefaultPermissionStore>();
            AddPermissionCore(services,action);
        }

        public static void AddPermissionCore(IServiceCollection services, Action<PermissionOptions> action)
        {
            #region 授权

            //权限控制只要在配置IServiceCollection，不需要额外配置app管道
            //权限控制参考：https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-2.2
            //handler和requirement有几种关系：1 handler对多requirement(此时handler实现IAuthorizationHandler)；1对1（实现AuthorizationHandler<PermissionRequirement>），和多对1
            //所有的handler都要注入到services，用services.AddSingleton<IAuthorizationHandler, xxxHandler>()，而哪个requirement用哪个handler，低层会自动匹配。最后将requirement对到policy里即可
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PermissionConstant.PermissionAuthorizePolicy, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new PermissionRequirement());
                });
            });
            services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
            services.Configure(action);
            #endregion
        }
    }
}
