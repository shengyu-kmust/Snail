using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Permission.Entity
{
    public class Org : BaseEntity
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
    }
}
