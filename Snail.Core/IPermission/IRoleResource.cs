using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.IPermission
{
    public interface IRoleResource
    {
        string GetRoleKey();
        string GetResourceKey();
    }
}
