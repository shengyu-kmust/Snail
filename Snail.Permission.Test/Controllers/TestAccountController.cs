using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Snail.Permission.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Permission.Test.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TestAccountController : AccountController<TestUser,Guid>
    //public class TestAccountController :ControllerBase
    {
        public TestAccountController(UserManager<TestUser> userManager, SignInManager<TestUser> signInManager, ILoggerFactory loggerFactory) : base(userManager, signInManager, loggerFactory)
        {
        }
      
         [HttpGet]
        public string Test1()
        {
            return "success";
        }

    }
}
