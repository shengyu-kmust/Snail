using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Entity
{
    [Table("Role")]
    public class Role:BaseEntity
    {
        public string RoleName { get; set; }
    }
}
