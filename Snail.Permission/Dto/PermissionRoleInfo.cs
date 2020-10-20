using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Dto
{
    public class PermissionRoleInfo:IRole
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string GetKey()
        {
            return Id;
        }

        public string GetName()
        {
            return Name;
        }

        public void SetName(string name)
        {
            this.Name = name;
        }
    }
}
