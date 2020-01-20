using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Snail.Common
{
    public class JwtTokenHelper
    {
        public bool TryGenerate(string key, string algName, out string tokenStr, string issuer = null, string audience = null, IEnumerable<Claim> claims = null, DateTime? notBefore = default(DateTime?), DateTime? expires = default(DateTime?))
        {
            try
            {
                var signingCredentials = new SigningCredentials(GenerateSecurityKey(key, algName), algName);
                var token = new JwtSecurityToken(issuer, audience, claims, notBefore, expires, signingCredentials);
                tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
                return true;
            }
            catch (Exception)
            {
                tokenStr = "";
                return false;
            }
        }

        public bool TryVerify(string tokenStr, string key, string algName, out SecurityToken validatedToken)
        {
            try
            {
                new JwtSecurityTokenHandler().ValidateToken(tokenStr, new TokenValidationParameters
                {
                    IssuerSigningKey = GenerateSecurityKey(key, algName)
                }, out SecurityToken validatedTokenTemp);
                validatedToken = validatedTokenTemp;
                return true;
            }
            catch (Exception)
            {
                validatedToken = null;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="algName"></param>
        /// <returns></returns>
        private SecurityKey GenerateSecurityKey(string key, string algName)
        {
            if (algName == "hs256")
            {
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));//key必须大于256个字节才行
            }
            else if (algName == "rs256")
            {
                return new RsaSecurityKey(new System.Security.Cryptography.RSAParameters());//通过rsa pem格式的key转成parameters
            }
            throw new NotSupportedException();
        }
    }
}
