using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Entity
{
    public class UserOrg<TKey>:BaseEntity<TKey>
    {
        public int UserId { get; set; }
        public int OrgId { get; set; }
    }
}
