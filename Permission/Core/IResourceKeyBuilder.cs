using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Core
{
    public interface IResourceKeyBuilder
    {
        string BuildKey(object obj);
    }
}
