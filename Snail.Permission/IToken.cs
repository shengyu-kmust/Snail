using System;
using System.Collections.Generic;

namespace Snail.Permission
{
    public interface IToken
    {
        List<System.Security.Claims.Claim> ResolveFromToken(string tokenStr);
        string ResolveToken(List<System.Security.Claims.Claim> claims, DateTime expireTime);
    }
}
