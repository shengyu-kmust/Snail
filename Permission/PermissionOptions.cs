using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission
{
    public class PermissionOptions
    {
        public string PasswordSalt { get; set; } = string.Empty;
        public string RsaPrivateKey { get; set; }
        public string RsaPublicKey { get; set; }
    }
}
