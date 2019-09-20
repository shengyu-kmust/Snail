using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Entity
{
    public class UserRole<TKey>:BaseEntity<TKey>
    {
        public virtual int UserId { get; set; }
        public virtual int RoleId { get; set; }
    }
}
