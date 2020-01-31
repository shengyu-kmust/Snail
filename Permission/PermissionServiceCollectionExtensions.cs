using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Snail.Common;
using Snail.Core.Permission;
using Snail.Entity;
using System;
namespace Snail.Permission
{
    public static class PermissionServiceCollectionExtensions
    {
        /// <summary>
        /// 自定义权限表，实现权限功能
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TRole"></typeparam>
        /// <typeparam name="TUserRole"></typeparam>
        /// <typeparam name="TResource"></typeparam>
        /// <typeparam name="TRoleResource"></typeparam>
        /// <param name="services"></param>
        /// <param name="action"></param>
        public static void AddPermission<TDbContext,TUser, TRole, TUserRole, TResource, TRoleResource>(this IServiceCollection services,Action<PermissionOptions> action)
         where TUser : class, IUser, new()
        where TRole : class, IRole, new()
        where TUserRole : class, IUserRole, new()
        where TResource : class, IResource, new()
        where TRoleResource : class, IRoleResource, new()
        where TDbContext:DbContext
        {
            services.TryAddScoped<IPermission, DefaultPermission<TDbContext,TUser, TRole, TUserRole, TResource, TRoleResource>>();
            services.TryAddScoped<IToken, Token>();
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

        /// <summary>
        /// 用默认的User, Role, UserRole, Resource, RoleResource表实现权限,即TDbContext已经有默认的这几张表，TDbContext可以通过继承PermissionDatabaseContext来简化实现过程
        /// </summary>
        /// <typeparam name="TDbContext">权限的表是在哪个dbcontext</typeparam>
        /// <param name="services"></param>
        /// <param name="action"></param>
        public static void AddPermission<TDbContext>(this IServiceCollection services, Action<PermissionOptions> action) where TDbContext:DbContext
        {
            services.AddPermission<TDbContext,User, Role, UserRole, Resource, RoleResource>(action);
        }
    }
}
