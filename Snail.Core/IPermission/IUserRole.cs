using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.IPermission
{
    public interface IUserRole
    {
        string GetUserKey();
        string GetRoleKey();
    }
}
