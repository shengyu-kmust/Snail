using Snail.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Core
{
    public class DefaultPermissionDataManager:IUserManager<User>,IRoleManager<Role>,IUserRoleManager<User,Role,UserRole>,IResourceManager<Resource>,IRoleResourceManager<>
    {
    }
}
