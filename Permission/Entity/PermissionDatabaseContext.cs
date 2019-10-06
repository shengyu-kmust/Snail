using Microsoft.EntityFrameworkCore;
using Snail.Abstract.Entity;
using Snail.Abstract.Enum;
using System;

namespace Snail.Entity
{
    public abstract class PermissionDatabaseContext<TUser,TRole,TUserRole,TResource,TPermission,TOrganization,TUserOrg,TKey> : DbContext 
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
                i.Property(u => u.Gender).HasColumnName("int").HasConversion<int>();
            });
            modelBuilder.Entity<TRole>(i => 
            {
                i.HasKey(r => r.Id);

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
            });
            modelBuilder.Entity<TUserOrg>(i =>
            {
                i.HasKey(uo => uo.Id);
            });
          
            modelBuilder.Entity<TPermission>(i =>
            {
                i.HasKey(p => p.Id);
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
