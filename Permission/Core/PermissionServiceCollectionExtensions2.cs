using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Snail.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Core
{
    /// <summary>
    /// 怎么用？
    /// 用法一：自己实现entity时，
    /// 用法二：默认完整用法
    /// </summary>
    public static class PermissionServiceCollectionExtensions2
    {
        /// <summary>
        /// 单独用时，请先注册IPermissionStore，IResourceKeyBuilder
        /// </summary>
        /// <param name="services"></param>
        public static void AddPermissionCore(this IServiceCollection services)
        {
            #region 身份验证
            //约定
            //1、身份验证以支持Jwt和cookie两种为主，先jwt再cookie验证
            //2、支持第三方openid connect登录，但第三方登录成功后，如果是web应用，则同时登录到cookie验证，如果是webapi应用，需在第三方登录成功后从系统获取jwt做后续的api调用
            services.AddAuthentication(PermissionConstant.PermissionScheme)
                .AddCookie(
                    PermissionConstant.PermissionScheme, options =>
                    {
                        options.AccessDeniedPath = new PathString("/test/AccessDeniedPath");
                        options.LoginPath = new PathString("/test/LoginPath");
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
                                return PermissionConstant.PermissionScheme;
                            }
                        };
                    })
                //.AddCookie(IdentityConstants.ApplicationScheme)//这句是配置identity的authenticateScheme，
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        NameClaimType = "userId",
                        RoleClaimType = "roleId",
                        ValidIssuer = "snail issuer",
                        ValidAudience = "snail audience",
                        IssuerSigningKey = new RsaSecurityKey(RSAHelper.GetRSAParametersFromFromPrivatePem(""))//todo
                    };
                });
            #endregion

            #region 授权

            //权限控制只要在配置IServiceCollection，不需要额外配置app管道
            //权限控制参考：https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-2.2
            //handler和requirement有几种关系：1 handler对多requirement(此时handler实现IAuthorizationHandler)；1对1（实现AuthorizationHandler<PermissionRequirement>），和多对1
            //所有的handler都要注入到services，用services.AddSingleton<IAuthorizationHandler, xxxHandler>()，而哪个requirement用哪个handler，低层会自动匹配。最后将requirement对到policy里即可
            //services.AddSingleton<PermissionManager>();
            // 授权策略
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PermissionConstant.PermissionAuthorizePolicy, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new PermissionRequirement());
                });
            });
            services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler2>();
            #endregion

        }

        public static void AddEFPermission<TUser,TRole,TUserRole,TResource,TRoleResource,TDbContext>(this IServiceCollection services)
        {
            #region 授权
            //services.AddScoped<IPermissionStore, DefaultEFPermissionStore<TUser, TRole, TUserRole, TResource, TRoleResource, TDbContext>>();
            services.AddPermissionCore();
            #endregion
        }

        /// <summary>
        /// 自己定义各类IStore的实现，并注册进DI
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TRole"></typeparam>
        /// <typeparam name="TUserRole"></typeparam>
        /// <typeparam name="TResource"></typeparam>
        /// <typeparam name="TRoleResource"></typeparam>
        /// <typeparam name="TOrg"></typeparam>
        /// <typeparam name="TUserOrg"></typeparam>
        /// <param name="services"></param>
        public static void AddPermission<TUser, TRole, TUserRole, TResource, TRoleResource,TOrg,TUserOrg>(this IServiceCollection services)
        {

        }

        /// <summary>
        /// 用ef
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TRole"></typeparam>
        /// <typeparam name="TUserRole"></typeparam>
        /// <typeparam name="TResource"></typeparam>
        /// <typeparam name="TRoleResource"></typeparam>
        /// <typeparam name="TOrg"></typeparam>
        /// <typeparam name="TUserOrg"></typeparam>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        public static void AddDefaultEFPermission<TUser, TRole, TUserRole, TResource, TRoleResource, TOrg, TUserOrg,TDbContext>(this IServiceCollection services)
        {
            services.AddScoped<IPermissionStore<TUser, TRole, TUserRole, TResource, TRoleResource, TOrg, TUserOrg>, DefaultEFPermissionStore<TUser, TRole, TUserRole, TResource, TRoleResource, TOrg, TUserOrg, TDbContext>>();
            
        }


    }
}
