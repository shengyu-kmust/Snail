using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Snail.Core.IPermission;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snail.Permission
{
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
    {
        private IPermission _permission;
        public PermissionRequirementHandler(IPermission permission)
        {
            _permission = permission;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var resourceKey=_permission.GetRequestResourceKey(context.Resource);
            var userKey = _permission.GetUserKey(context.User);
            if (_permission.HasPermission(resourceKey,userKey))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
