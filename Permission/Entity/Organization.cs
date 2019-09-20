using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Entity
{
    public class Organization<TKey>:BaseEntity<TKey>
    {
        public virtual string Name { get; set; }
        public virtual TKey ParentId { get; set; }
    }
}
