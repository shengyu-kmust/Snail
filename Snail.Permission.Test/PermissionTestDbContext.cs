using Microsoft.EntityFrameworkCore;
using Snail.Entity;
using Snail.Permission.Test.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace Snail.Permission.Test
{
    public class PermissionTestDbContext : DbContext
    //public class PermissionTestDbContext : PermissionDatabaseContext<UserTest, RoleTest, UserRole, Resource, Entities.Permission, Organization, UserOrg, Guid>
    //public class PermissionTestDbContext : PermissionDatabaseContext<UserTest, RoleTest, UserRole, Resource, Entities.Permission, Organization,  Guid>
    {
        public DbSet<UserTest> Users { get; set; }
        public DbSet<UserOrg> UserOrgs { get; set; }
        public PermissionTestDbContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserTest>(i =>
            {
                i.HasKey(u => u.Id);
                //i.HasMany<TUserRole>("UserRoles").WithOne("User").HasForeignKey(a => a.UserId);
                //i.HasMany<TUserOrg>("UserOrgs").WithOne("User").HasForeignKey(a => a.UserId);
            });
        }
    }
}
