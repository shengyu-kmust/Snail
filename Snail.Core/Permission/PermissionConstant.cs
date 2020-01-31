using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Permission
{
    public class PermissionConstant
    {
        public const string PermissionAuthorizePolicy = nameof(PermissionAuthorizePolicy);
        public const string PermissionScheme = nameof(PermissionScheme);
        public const string userIdClaim = "userId";
        public const string userNameClaim = "userName";
        public const string accountClaim = "account";
        public const string roleIdsClaim = "roleIds";
        public const string rolesNameClaim = "roleNames";
        public static readonly TimeSpan tokenExpire = new TimeSpan(6, 0, 0);
    }
}
