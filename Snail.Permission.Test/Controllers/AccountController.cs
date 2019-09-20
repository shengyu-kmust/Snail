using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Permission.Test.Controllers
{
    public class AccountController
    {
        private PermissionTestDbContext _dbContext;
        public AccountController(PermissionTestDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public string Test()
        {
            //var users = _dbContext.Users.ToList();
            return "success";
        }
    }
}
