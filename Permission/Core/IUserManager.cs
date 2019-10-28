using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Core
{
    public interface IUserManager
    {
        string GetTokenByAccountAndPassword(string account, string password);
        string GetTokenByPhoneAndCode(string phone, string code);
        /// <summary>
        /// 登录并cockies
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool SignInByAccountAndPassword(string account);
        bool SignInByPhoneAndCode(string account);
        void Registor(object registorDto);
    }
}
