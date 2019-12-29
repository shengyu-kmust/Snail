using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Snail.Common;
using Snail.Core.IPermission;
using Snail.Entity;
using System;
namespace Snail.Permission
{
    public static class PermissionServiceCollectionExtensions
    {
        public static void AddPermission<TUser, TRole, TUserRole, TResource, TRoleResource>(this IServiceCollection services)
         where TUser : class, IUser, new()
        where TRole : class, IRole, new()
        where TUserRole : class, IUserRole, new()
        where TResource : class, IResource, new()
        where TRoleResource : class, IRoleResource, new()
        {
            services.TryAddScoped<IPermission, DefaultPermission<TUser, TRole, TUserRole, TResource, TRoleResource>>();
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
            #endregion
        }

        public static void AddPermission(this IServiceCollection services)
        {
            services.AddPermission<User, Role, UserRole, Resource, RoleResource>();
        }
    }
}
