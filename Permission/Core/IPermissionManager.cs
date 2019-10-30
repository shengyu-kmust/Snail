using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Core
{
    public interface IPermissionManager<TUser, TRole, TUserRole, TResource, TPermission, TOrganization, TUserOrg>
    {
        string GetToken(string account, string password);
        void SignIn(string account, string password);

        void Registor(string account,string password);

    }
}
