using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Entity
{
    public class Permission<TKey> : BaseEntity<TKey>
    {
        public TKey RoleId { get; set; }
        public TKey ResourceId { get; set; }
    }
}
