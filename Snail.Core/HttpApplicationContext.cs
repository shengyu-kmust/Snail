using Microsoft.AspNetCore.Http;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core
{
    public class HttpApplicationContext : IApplicationContext
    {
        private IHttpContextAccessor _httpContextAccessor;
        public HttpApplicationContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
