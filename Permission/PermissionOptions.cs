using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission
{
    public class PermissionOptions
    {
        public IdentityOptions Identity { get; set; } = new IdentityOptions();
        public string PasswordSalt { get; set; } = string.Empty;
    }
}
