using Snail.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Pwd { get; set; }
        public EGender Gender { get; set; }

        public string Creater { get; set; }
        public string CreaterName { get; set; }
        public DateTime CreateTime { get; set; }
        public string Updater { get; set; }
        public string UpdaterName { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
