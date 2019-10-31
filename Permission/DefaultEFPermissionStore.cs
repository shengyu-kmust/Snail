using Microsoft.EntityFrameworkCore;
using Snail.Permission.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission
{
    public class DefaultEFPermissionStore<TUser, TRole, TUserRole, TResource, TRoleResource,TOrg,TUserOrg,TDbContext> : IPermissionStore<TUser, TRole, TUserRole, TResource, TRoleResource, TOrg, TUserOrg>
    {
        public List<TOrg> GetAllOrg()
        {
            throw new NotImplementedException();
        }

        public List<TRole> GetAllRole()
        {
            throw new NotImplementedException();
        }

        public List<TUser> GetAllUser()
        {
            throw new NotImplementedException();
        }

        public List<TUserOrg> GetAllUserOrg()
        {
            throw new NotImplementedException();
        }

        public List<TUserRole> GetAllUserRole()
        {
            throw new NotImplementedException();
        }

        public List<TResource> GetResource()
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
