using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Core
{
    public interface IUserOrgManager<TOrg,TUserOrg,TUser>
    {
        void SaveOrg(TOrg org);
        void RemoveOrg(TOrg org);
        List<TOrg> GetAllOrgs();

        void SaveOrgUser(TUserOrg userOrg);
        List<TUser> GetOrgUsers(TOrg org);
    }
}
