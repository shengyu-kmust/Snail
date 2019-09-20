using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Entity
{
    public class Permission<TKey> : BaseEntity<TKey>
    {
        public virtual TKey RoleId { get; set; }
        public virtual TKey ResourceId { get; set; }
    }
}
