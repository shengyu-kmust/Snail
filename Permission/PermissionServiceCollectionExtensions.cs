using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snail.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission
{
    public static class PermissionServiceCollectionExtensions
    {
        public static void AddPermission<TUser, TRole, TUserRole, TResource, TPermission, TOrganization, TUserOrg, TContext, TKey>(this IServiceCollection services, Action<IdentityOptions> identityOptionsAction)
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
            var identityBuilder = identityOptionsAction == null ? services.AddIdentityCore<TUser>(a => { }) : services.AddIdentityCore<TUser>(identityOptionsAction).AddRoles<TRole>()

            #endregion

        }
    }
}
