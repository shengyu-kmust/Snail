using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Dto
{
    public class SetRoleResourceDto
    {
        public string RoleKey { get; set; }
        public List<string> ResourceKeys { get; set; }
    }
}
