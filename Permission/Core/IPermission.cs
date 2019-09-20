using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Core
{
    public interface IPermission<TUser,TRole,TResource>
    {
        bool HasPermission(TUser user, TResource resource);
    }
}
