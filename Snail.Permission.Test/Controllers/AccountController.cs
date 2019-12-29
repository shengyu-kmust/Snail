using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Permission.Test.Controllers
{
    [Route("[Controller]/[Action]")]
    public class AccountController
    {
        

        [HttpGet]
        public string Test()
        {
            //var users = _dbContext.Users.ToList();
            return "success";
        }
    }
}
