using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Entity
{

    [Table("UserRole")]
    public class UserRole<TKey>:BaseEntity<TKey>
    {
        public TKey UserId { get; set; }
        public TKey RoleId { get; set; }
    }
}
