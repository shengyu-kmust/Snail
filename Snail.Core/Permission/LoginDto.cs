using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Permission
{
    public class LoginDto
    {
        public string Account { get; set; }
        public string Pwd { get; set; }
        /// <summary>
        /// 是否登录到cookie
        /// </summary>
        public bool SignIn { get; set; }
    }
}
