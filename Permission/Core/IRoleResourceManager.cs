using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Core
{
    public interface IRoleResourceManager<TRoleResource>
    {
        void Save(TRoleResource resource);
        void Delete(TRoleResource resource);
    }
}
