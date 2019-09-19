using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Snail.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission
{
    public static class PermissionServiceCollectionExtensions
    {
        public static void AddPermission<TUser, TRole, TUserRole, TResource, TPermission, TOrganization, TUserOrg, TContext, TKey>(this IServiceCollection services)
        where TUser : User<TKey>
        where TRole : Role<TKey>
        where TUserRole : UserRole<TKey>
        where TResource : Resource<TKey>
        where TPermission : Permission<TKey>
        where TOrganization : Organization<TKey>
        where TUserOrg : UserOrg<TKey>
        where TContext : DbContext

        {
            #region 增加identity功能
            services.TryAddScoped<IPasswordHasher<TUser>, PermissionPasswordHasher<TUser>>();
            services.AddScoped<IUserStore<TUser>, PermissionUserStore<TUser, TContext, TKey>>();

            services.AddIdentityCore<TUser>().AddRoles<TRole>();
            #endregion

            #region 身份验证
            //约定
            //1、身份验证以支持Jwt和cookie两种为主，先jwt再cookie验证
            //2、支持第三方openid connect登录，但第三方登录成功后，如果是web应用，则同时登录到cookie验证，如果是webapi应用，需在第三方登录成功后从系统获取jwt做后续的api调用
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    CookieAuthenticationDefaults.AuthenticationScheme, options =>
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
                                return CookieAuthenticationDefaults.AuthenticationScheme;
                            }
                        };
                    })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        NameClaimType = "userId",
                        RoleClaimType = "roleId",
                        ValidIssuer = "snail issuer",
                        ValidAudience = "snail audience",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("snail"))
                    };
                });
            #endregion

        }
    }
}
