using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core.Interface;
using Snail.Core.Permission;
using Snail.Permission.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Snail.Permission
{
    public class DefaultPermissionStore : BasePermissionStore<DbContext, PermissionDefaultUser , PermissionDefaultRole, PermissionDefaultUserRole , PermissionDefaultResource , PermissionDefaultRoleResource >, IPermissionStore
    {
        public DefaultPermissionStore(DbContext db, IMemoryCache memoryCache, IOptionsMonitor<PermissionOptions> permissionOptions, IApplicationContext applicationContext) : base(db, memoryCache, permissionOptions, applicationContext)
        {
        }
    }

  
}
