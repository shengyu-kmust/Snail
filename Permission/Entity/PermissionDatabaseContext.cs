using Microsoft.EntityFrameworkCore;
using Snail.Core.Entity;
using Snail.Core.Enum;
using System;

namespace Snail.Permission.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PermissionDatabaseContext : DbContext 
    {
        #region 通用权限表
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoleses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<RoleResource> RoleResources { get; set; }
        public DbSet<Org> Orgs { get; set; }
        public DbSet<UserOrg> UserOrgs { get; set; }
        #endregion

        public PermissionDatabaseContext()
        {

        }
        public PermissionDatabaseContext(DbContextOptions options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().Property(a => a.Gender).HasConversion<string>();
        }
    }
}
