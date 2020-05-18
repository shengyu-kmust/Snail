using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Snail.Common;
using Snail.Core.Default;
using Snail.Core.Interface;
using Snail.Core.Permission;
using Snail.Permission.Entity;
using System;
using System.Text;

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
        public static void AddDefaultPermission<TDbContext>(this IServiceCollection services, Action<PermissionOptions> action) where TDbContext : DbContext
        {
            if (!typeof(PermissionDatabaseContext).IsAssignableFrom(typeof(TDbContext)))
            {
                throw new Exception($"{typeof(TDbContext).Name}未继承{typeof(PermissionDatabaseContext).Name}");
            }
            services.TryAddScoped<DbContext, TDbContext>();
            services.TryAddScoped<IPermission, DefaultPermission>();
            services.TryAddScoped<IPermissionStore, DefaultPermissionStore>();
            AddPermissionCore(services, action);
        }


        /// <summary>
        /// 自定义TUser,TRole,TUserRole,TResource,TRoleResource，需自己实现和注册IPermissionStore
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TRole"></typeparam>
        /// <typeparam name="TUserRole"></typeparam>
        /// <typeparam name="TResource"></typeparam>
        /// <typeparam name="TRoleResource"></typeparam>
        /// <param name="services"></param>
        /// <param name="action"></param>
        public static void AddPermission<TDbContext, TUser, TRole, TUserRole, TResource, TRoleResource>(this IServiceCollection services, Action<PermissionOptions> action)
          where TDbContext : DbContext
          where TUser : class, IUser, new()
          where TRole : class, IRole, new()
          where TUserRole : class, IUserRole, new()
          where TResource : class, IResource, new()
          where TRoleResource : class, IRoleResource, new()
        {
            services.TryAddScoped<DbContext, TDbContext>();
            services.TryAddScoped<IPermission, DefaultPermission>();
            AddPermissionCore(services, action);
        }


        /// <summary>
        /// 除User表用TUser外,其它的表都和AddDefaultPermission一样，需自己实现和注册IPermissionStore
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="services"></param>
        /// <param name="action"></param>
        public static void AddPermission<TDbContext, TUser>(this IServiceCollection services, Action<PermissionOptions> action)
          where TDbContext : DbContext
          where TUser : class, IUser, new()
        {
            services.TryAddScoped<DbContext, TDbContext>();
            services.TryAddScoped<IPermission, DefaultPermission>();
            AddPermissionCore(services, action);
        }


        /// <summary>
        /// 权限控制核心，即必须的配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        public static void AddPermissionCore(this IServiceCollection services, Action<PermissionOptions> action)
        {
            #region MyRegion
            var permissionOption = new PermissionOptions();
            action(permissionOption);
            //addAuthentication不放到AddPermissionCore方法里，是为了外部可自己配置
            // 当未通过authenticate时（如无token或是token出错时），会返回401，当通过了authenticate但没通过authorize时，会返回403。
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(
                   CookieAuthenticationDefaults.AuthenticationScheme, options =>
                   {
                   //下面的委托方法只会在第一次cookie验证时调用，调用时会用到上面的permissionOption变量，但其实permissionOption变量是在以前已经初始化的，所以在此方法调用之前，permissionOption变量不会被释放
                   options.Cookie.Name = "auth";
                       options.AccessDeniedPath = permissionOption.AccessDeniedPath;
                       options.LoginPath = permissionOption.LoginPath;
                       options.ExpireTimeSpan = permissionOption.ExpireTimeSpan != default ? permissionOption.ExpireTimeSpan : new TimeSpan(12, 0, 0);
                       options.ForwardDefaultSelector = context =>
                       {
                           string authorization = context.Request.Headers["Authorization"];
                       //身份验证的顺序为jwt、cookie
                       if (authorization != null && authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                           {
                               return JwtBearerDefaults.AuthenticationScheme;
                           }
                           else
                           {
                               return CookieAuthenticationDefaults.AuthenticationScheme;
                           }
                       };
                   })
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
               {
                   SecurityKey key;
                   if (permissionOption.IsAsymmetric)
                   {
                       key = new RsaSecurityKey(RSAHelper.GetRSAParametersFromFromPublicPem(permissionOption.RsaPublicKey));
                   }
                   else
                   {
                       key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(permissionOption.SymmetricSecurityKey));
                   }
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {

                       NameClaimType = PermissionConstant.userIdClaim,
                       RoleClaimType = PermissionConstant.roleIdsClaim,
                       ValidIssuer = permissionOption.Issuer,
                       ValidAudience = permissionOption.Audience,
                       IssuerSigningKey = key,
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };
               });
            #endregion
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
            services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
            services.AddMemoryCache();
            services.TryAddScoped<IApplicationContext, ApplicationContext>();
            services.AddHttpContextAccessor();
            services.Configure(action);
            #endregion
        }

    }
}
