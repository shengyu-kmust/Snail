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
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public Role<TKey> Role { get; set; }
        public User<TKey> User { get; set; }
    }
}
