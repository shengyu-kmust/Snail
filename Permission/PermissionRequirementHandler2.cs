using Microsoft.AspNetCore.Authorization;
using Snail.Permission.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snail.Permission
{
    public class PermissionRequirementHandler2 : AuthorizationHandler<PermissionRequirement>
    {
        //private PermissionManager _permissionManager;
        //public PermissionRequirementHandler2(PermissionManager permissionManager)
        //{
        //    _permissionManager =permissionManager;
        //}
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //if (_permissionManager.HasPermission(_permissionManager.GetUserKey(context.User), _permissionManager.GetResourceKey(context.Resource)))
            //{
            //    context.Succeed(requirement);
            //}
            return Task.CompletedTask;
        }
    }
}
