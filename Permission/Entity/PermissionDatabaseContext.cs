using Microsoft.EntityFrameworkCore;
using Snail.Common;
using System;
using System.Security.Cryptography;
using System.Text;

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
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var userId = IdGenerator.Generate<string>();
            var roleId = IdGenerator.Generate<string>();
            var pwdHash=BitConverter.ToString(HashAlgorithm.Create(HashAlgorithmName.MD5.Name).ComputeHash(Encoding.UTF8.GetBytes("123456"))).Replace("-", "");
            var now = DateTime.Now;
            modelBuilder.Entity<User>().HasData(new User { Id = userId, Account = "SuperAdmin", CreateTime = now, IsDeleted = false, Name = "超级管理员", Pwd = pwdHash });
            modelBuilder.Entity<Role>().HasData(new Role { Id = roleId ,Name=DefaultPermission.superAdminRoleName,CreateTime= now ,IsDeleted=false});
            modelBuilder.Entity<UserRole>().HasData(new UserRole { Id= IdGenerator.Generate<string>() ,IsDeleted=false,RoleId=roleId,UserId=userId,CreateTime=now});
        }
    }
}
