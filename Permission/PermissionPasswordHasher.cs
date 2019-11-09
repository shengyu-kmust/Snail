using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// 要修改
/// </summary>
namespace Snail.Permission
{
    public class PermissionPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser:class
    {
        private PermissionOptions _options;
        public PermissionPasswordHasher(IOptions<PermissionOptions> optionsAccessor)
        {
            _options = optionsAccessor?.Value ?? new PermissionOptions();
        }
        public string HashPassword(TUser user, string password)
        {
            return BitConverter.ToString(MD5.Create().ComputeHash(UTF8Encoding.UTF8.GetBytes($"{password}{_options.PasswordSalt}"))).Replace("-", "");
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            return HashPassword(user, providedPassword) == hashedPassword ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
