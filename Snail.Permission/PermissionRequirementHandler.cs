﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Snail.Core.Permission;
using System;
using System.Linq;
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
            /*
             权限最佳做法：
            1、所有Controller继承拥有AuthorizeAttribute的BaseController
            2、不用控制权限的Action加AllowAnonymousAttribute
            3、要进行特殊授权的，用[Authorize(Policy=PermissionConstant.PermissionAuthorizePolicy)]

             
             */
            //if (context.Requirements.Any(a=>a is OnlyAuthenticationRequirement))
            //{
            //    foreach (var req in context.Requirements)
            //    {
            //        context.Succeed(req);
            //    }
            //    return Task.CompletedTask;
            //}
            if (context.User?.Identity?.IsAuthenticated??false)
            {
                var resourceKey = _permission.GetRequestResourceKey(context.Resource);// 获取资源的key
                var userKey = _permission.GetUserInfo(context.User).UserKey; // 根据用户的claims获取用户的key
                if (_permission.HasPermission(resourceKey, userKey)) // 判断用户是否有权限
                {
                    context.Succeed(requirement); // 如果有权限，则获得此Requirement
                }
            }
            return Task.CompletedTask;
        }
    }
}
