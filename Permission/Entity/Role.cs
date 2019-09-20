using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Entity
{
    public class InnerRole<TKey> : BaseEntity<TKey>
    {
        public virtual string RoleName { get; set; }
    }


}
