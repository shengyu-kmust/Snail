using Microsoft.EntityFrameworkCore;
using Snail.Permission.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission
{
    public class EFPermissionStore<TUser,TRole,TUserRole,TResource,TRoleResource,TDbContext> : IPermissionStore
    {

        public List<string> GetResourceKeys(TDbContext db)
        {
            throw new NotImplementedException();
        }

        public List<string> GetResourceKeys()
        {
            throw new NotImplementedException();
        }

        public List<string> GetRoleKeys()
        {
            throw new NotImplementedException();
        }

        public List<(string roleKey, List<string> roleResources)> GetRoleResources()
        {
            throw new NotImplementedException();
        }

        public List<string> GetUserKeys()
        {
            throw new NotImplementedException();
        }

        public List<(string userKey, string roleKey)> GetUserRoles()
        {
            throw new NotImplementedException();
        }
    }
}
