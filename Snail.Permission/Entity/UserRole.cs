using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Permission.Entity
{

    [Table("UserRole")]
    public class UserRole:BaseEntity,IUserRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public string GetRoleKey()
        {
            return this.RoleId;
        }

        public string GetUserKey()
        {
            return this.UserId;
        }
    }
}
