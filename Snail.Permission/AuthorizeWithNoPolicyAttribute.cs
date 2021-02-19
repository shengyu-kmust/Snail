using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission
{
    /// <summary>
    /// 只要标记这个特性，就不要求进入policy的权限校验，只验证用户登录了即可
    /// </summary>
    public class AuthorizeWithNoPolicyAttribute : Attribute
    {
    }
}
