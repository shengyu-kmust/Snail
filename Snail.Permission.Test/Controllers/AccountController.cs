using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Core.Attributes;
using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Permission.Test.Controllers
{
    [ApiController,Route("[Controller]/[Action]"),Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    [Resource(Description ="测试菜单")]
    public class AccountController:ControllerBase
    {
        [HttpGet]
        [Resource(Description ="测试action")]
        public string Test()
        { 
            //var users = _dbContext.Users.ToList();

            return "success";
        }

        [HttpGet]
        [Resource(Description ="测试action2")]
        public string Test2()
        {
            return "success";
        }
    }
}
