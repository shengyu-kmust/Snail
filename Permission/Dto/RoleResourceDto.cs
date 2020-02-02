using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Dto
{
    public class RoleResourceDto
    {
        public string RoleKey { get; set; }
        public List<string> ResourceKeys { get; set; }
    }
}
