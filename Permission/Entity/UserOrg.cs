using Snail.Core.Permission;
using System;

namespace Snail.Permission.Entity
{
    public class UserOrg:BaseEntity,IUserOrg
    {
        public string UserId { get; set; }
        public string OrgId { get; set; }

        public string GetOrgKey()
        {
            throw new NotImplementedException();
        }

        public string GetUserKey()
        {
            throw new NotImplementedException();
        }
    }
}
