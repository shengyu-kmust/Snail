using Snail.Abstract.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snail.Entity
{
    [Table("User")]
    public class User<TKey>:BaseEntity<TKey>
    {
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Pwd { get; set; }
        public Gender Gender { get; set; }   
    }

}
