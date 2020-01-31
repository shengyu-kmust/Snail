using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Permission
{
    public interface IUserRole
    {
        string GetUserKey();
        string GetRoleKey();
    }
}
