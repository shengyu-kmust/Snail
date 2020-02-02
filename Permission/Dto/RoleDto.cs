using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Dto
{
    public class RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Creater { get; set; }
        public string CreaterName { get; set; }
        public DateTime CreateTime { get; set; }
        public string Updater { get; set; }
        public string UpdaterName { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
