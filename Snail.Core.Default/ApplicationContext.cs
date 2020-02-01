using Microsoft.AspNetCore.Http;
using Snail.Core.Interface;
using Snail.Core.Permission;
using System;

namespace Snail.Core.Default
{
    public class ApplicationContext : IApplicationContext
    {
        private IHttpContextAccessor _httpContextAccessor;
        public ApplicationContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }
        public string GetCurrentUserId()
        {
            return _httpContextAccessor?.HttpContext?.User?.FindFirst(a => a.Type.Equals(PermissionConstant.userIdClaim, StringComparison.OrdinalIgnoreCase))?.Value;
        }
    }
}
