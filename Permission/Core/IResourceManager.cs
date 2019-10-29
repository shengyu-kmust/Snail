using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Core
{
    public interface IResourceManager<TResource>
    {
        void InitAllResource();
        void SaveResource(TResource resource);
        void DeleteResource(TResource resource);
    }
}
