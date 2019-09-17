using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Entity
{
    public class Organization<TKey>:BaseEntity<TKey>
    {
        public string Name { get; set; }
        public TKey ParentId { get; set; }
    }
}
