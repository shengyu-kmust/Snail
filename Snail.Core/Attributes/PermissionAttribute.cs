using Snail.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Attributes
{
    public class PermissionAttribute:Attribute
    {
        public string ResourceCode { get; set; }
        public string Description { get; set; }
        public EResourceType ResourceType { get; set; }
    }
}
