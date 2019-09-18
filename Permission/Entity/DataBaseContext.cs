using Microsoft.EntityFrameworkCore;
using Snail.Abstract.Entity;
using System;

namespace Snail.Entity
{
    public class PermissionDatabaseContext<TUser,TRole,TUserRole,TResource,TPermission,TOrganization,TUserOrg,TKey> : DbContext 
        where TUser: User<TKey>
        where TRole: Role<TKey>
        where TUserRole: UserRole<TKey>
        where TResource : Resource<TKey>
        where TPermission : Permission<TKey>
        where TOrganization : Organization<TKey>
        where TUserOrg : UserOrg<TKey>
    {
        #region 通用权限表
        public DbSet<TUser> Users { get; set; }
        public DbSet<TRole> Roles { get; set; }
        public DbSet<TUserRole> UserRoleses { get; set; }
        public DbSet<TResource> Resources { get; set; }
        public DbSet<TPermission> Permissions { get; set; }
        public DbSet<TOrganization> Organizations { get; set; }
        public DbSet<TUserOrg> UserOrgs { get; set; }
        #endregion
    

      
        public PermissionDatabaseContext(DbContextOptions options)
            : base(options)
        {

        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TUser>(i =>
            {
                i.HasKey(u => u.Id);
                i.HasMany<TUserRole>("UserRoles").WithOne("User").HasForeignKey(a => a.UserId);
                i.HasMany<TUserOrg>("UserOrgs").WithOne("User").HasForeignKey(a => a.UserId);
            });
            modelBuilder.Entity<TRole>(i => 
            {
                i.HasKey(r => r.Id);
                i.HasMany<TUserRole>("RoleUsers").WithOne("Role").HasForeignKey(a => a.RoleId);
                i.HasMany<TPermission>("Permissions").WithOne("Role").HasForeignKey(a => a.RoleId);

            });
            modelBuilder.Entity<TOrganization>(i =>
            {
                i.HasKey(o => o.Id);
            });
            modelBuilder.Entity<TResource>(i =>
            {
                i.HasKey(r => r.Id);
            });
            modelBuilder.Entity<TUserRole>(i =>
            {
                i.HasKey(ur => ur.Id);
                i.HasOne<TUser>("User").WithMany("UserRoles").HasForeignKey(a => a.UserId);
                i.HasOne<TRole>("Role").WithMany("RoleUsers").HasForeignKey(a => a.RoleId);
            });
            modelBuilder.Entity<TUserOrg>(i =>
            {
                i.HasKey(uo => uo.Id);
                i.HasOne<TUser>("User").WithMany("UserOrgs").HasForeignKey(a => a.UserId);
                i.HasOne<TOrganization>("Org").WithMany("Users").HasForeignKey(a => a.OrgId);
            });
          
            modelBuilder.Entity<TPermission>(i =>
            {
                i.HasKey(p => p.Id);
                i.HasOne<TRole>("Role").WithMany("Permissions").HasForeignKey(a => a.RoleId);
                i.HasOne<TResource>("Resource").WithMany().HasForeignKey(a => a.ResourceId);
            });


        }

        /// <summary>
        /// 设置公共字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelBuilder"></param>
        private void SetBaseEntity<T>(ModelBuilder modelBuilder) where T : BaseEntity<TKey>
        {
            modelBuilder.Entity<T>(i =>
            {
                i.Property(a => a.CreateTime).HasDefaultValue(DateTime.Now).ValueGeneratedOnAdd();
                i.Property(a => a.UpdateTime).HasDefaultValue(DateTime.Now)
                    .ValueGeneratedOnAddOrUpdate();
                i.Property(a => a.IsDeleted).HasDefaultValue(false)
                    .ValueGeneratedOnAdd();
            });
        }

    }
}
