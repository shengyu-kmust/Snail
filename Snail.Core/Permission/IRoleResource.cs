using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Permission
{
    public interface IRoleResource
    {
        string GetRoleKey();
        string GetResourceKey();
    }
}
