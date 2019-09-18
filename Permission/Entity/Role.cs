using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Entity
{
    [Table("Role")]
    public class Role<TKey>:BaseEntity<TKey>
    {
        public string RoleName { get; set; }

        #region 导航属性
        public List<UserRole<TKey>> RoleUsers { get; set; }
        public List<Permission<TKey>> Permissions { get; set; }
        #endregion
    }
}
