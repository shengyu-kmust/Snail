using Microsoft.EntityFrameworkCore;
using Snail.Database;
using Snail.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Permission.Test
{
    public class TestDbContext : PermissionDatabaseContext
    {
        public TestDbContext()
        {

        }
        public TestDbContext(DbContextOptions options) : base(options)
        {
        }
     
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            else
            {
                optionsBuilder.UseMySql(ConnectStringHelper.ConnForMySql("localhost", "testPermission", "root", "root"));
            }

        }
    }
}
