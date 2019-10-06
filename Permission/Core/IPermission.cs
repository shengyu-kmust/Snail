using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Snail.Permission.Core
{
    public interface IPermission
    {
        bool HasPermission(ClaimsPrincipal user,Object resource);

    }
}
