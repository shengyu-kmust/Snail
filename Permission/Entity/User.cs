using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snail.Permission.Entity
{
    public class User:BaseEntity,IUser
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Pwd { get; set; }
        public int Gender { get; set; }

        public IUser Create(string account, string password,string id)
        {
            return new User { Account = account, Pwd = password,Id=id };
        }

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
    }
}
