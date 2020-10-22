using Snail.Core;
using Snail.Core.Permission;

namespace Snail.Permission.Dto
{
    /// <summary>
    /// 权限用户信息，用于默认的权限api dto
    /// </summary>
    public class PermissionUserInfo:IUser, IIdField<string>
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

        public void SetAccount(string account)
        {
            this.Account = account;
        }

        public void SetKey(string key)
        {
            this.Id = key;
        }

        public void SetName(string name)
        {
            this.Name = name;
        }

        public void SetPassword(string pwd)
        {
            this.Pwd = pwd;
        }
    }
}
