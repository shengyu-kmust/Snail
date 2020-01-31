using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Permission
{
    public class LoginResult
    {
        public UserInfo UserInfo { get; set; }
        public string Token { get; set; }
    }
}
