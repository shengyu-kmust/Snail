using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Permission.Test.Controllers
{
    [Route("[Controller]/[Action]"),Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    [Permission(Description ="测试菜单")]
    public class AccountController:ControllerBase
    {
        [HttpGet]
        [Permission(Description ="测试action")]
        [AllowAnonymous]
        public string Test()
        {
            //var users = _dbContext.Users.ToList();

            return "success";
        }
    }
}
