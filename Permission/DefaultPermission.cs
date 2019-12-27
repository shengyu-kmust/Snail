using Microsoft.EntityFrameworkCore;
using Snail.Core.IPermission;
using Snail.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission
{
    public class DefaultPermission : IPermission
    {
        public DbContext dbContext;
        public DefaultPermission(DbContext dbContext)
        {

        }
        public string GenerateResourceCode(object obj)
        {
            throw new NotImplementedException();
        }

        public List<IResourceRole> GetAllResourceRoles()
        {
            throw new NotImplementedException();
        }

        public string GetLoginToken(string account, string pwd)
        {
            throw new NotImplementedException();
        }

        public IUserInfo GetUserInfo(string token)
        {
            throw new NotImplementedException();
        }

        public bool HasPermission(string resourceCode, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
