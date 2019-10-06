using Microsoft.EntityFrameworkCore;
using Snail.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Permission.Test
{
    public class TestDbContext: PermissionDatabaseContext<TestUser,TestRole,TestUserRole,TestResource,TestPermission,TestOrganization,TestUserOrg,Guid>
    {
        public TestDbContext(DbContextOptions options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
