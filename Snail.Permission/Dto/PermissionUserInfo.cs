using Snail.Core.Enum;
using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Dto
{
    public class PermissionUserInfo:IUser
    {
        public string Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Pwd { get; set; }

        public string GetAccount()
        {
            return Account;
        }

        public string GetKey()
        {
            return Id;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetPassword()
        {
            return Pwd;
        }

        public void SetName(string name)
        {
            this.Name = name;
        }
    }
}
