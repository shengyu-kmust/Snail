using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Snail.Core.Attributes;
using Snail.Core.Default;
using Snail.Core.Permission;
using Snail.Permission.Entity;
using System;
using System.Linq;
using System.Reflection;

namespace Snail.Web
{
    public abstract class BaseAppDbContext<TUser,TRole,TResource,TUserRole,TRoleResource,TOrg,TUserOrg> : DbContext
        where TUser : class, IUser, new()
        where TRole : class, IRole, new()
        where TUserRole : class, IUserRole, new()
        where TResource : class, IResource, new()
        where TRoleResource : class, IRoleResource, new()
        where TOrg : class, IOrg, new()
        where TUserOrg : class, IUserOrg, new()
    {
        #region 通用权限表
        public DbSet<TUser> Users { get; set; }
        public DbSet<TRole> Roles { get; set; }
        public DbSet<TUserRole> UserRoles { get; set; }
        public DbSet<TResource> Resources { get; set; }
        public DbSet<TRoleResource> RoleResources { get; set; }
        public DbSet<TOrg> Orgs { get; set; }
        public DbSet<TUserOrg> UserOrgs { get; set; }
        #endregion
        #region 公共表
        public DbSet<Snail.Web.Entities.Config> Configs { get; set; }
        public DbSet<Snail.FileStore.FileInfo> FileInfos { get; set; }
        #endregion

        protected ICapPublisher _publisher;
        public BaseAppDbContext(DbContextOptions options, ICapPublisher publisher)
            : base(options)
        {
            _publisher = publisher;
        }

        public BaseAppDbContext(DbContextOptions options)
          : base(options)
        {
        }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 自动应用所有的IEntityTypeConfiguration配置
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public override int SaveChanges()
        {
            //统一在数据库上下文的操作前，触发缓存实体的数据清空。
            if (_publisher != null)
            {
                this.ChangeTracker.Entries().Where(a =>(a.State == EntityState.Added || a.State == EntityState.Modified || a.State == EntityState.Deleted) && Attribute.IsDefined(a.Entity.GetType(), typeof(EnableEntityCacheAttribute))).Select(a => a.Entity.GetType().Name).Distinct().ToList().ForEach(entityName =>
                {
                    _publisher.Publish(EntityCacheManager.EntityCacheEventName, new EntityChangeEvent { EntityName = entityName });
                });
            }

            return base.SaveChanges();
        }

        // 不用要SeedData会数据初始化，此方法会在每次migration时删除和创建数据
    }
}
