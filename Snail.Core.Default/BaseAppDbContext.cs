﻿using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Snail.Core.Attributes;
using Snail.Core.Entity;
using Snail.Core.Interface;
using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Snail.Core.Default
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
        public DbSet<Config> Config { get; set; }
        #endregion

        protected ICapPublisher _publisher;
        protected IApplicationContext _applicationContext;
        public BaseAppDbContext(DbContextOptions options, ICapPublisher publisher, IApplicationContext applicationContext)
            : base(options)
        {
            _publisher = publisher;
            _applicationContext = applicationContext;
        }

        public BaseAppDbContext(DbContextOptions options)
          : base(options)
        {
        }



        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 自动应用所有的IEntityTypeConfiguration配置的方法示例如下
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly);//，在外层配置

            // 增加QueryFilter的方法示例如下
            //if (typeof(ITenant<string>).IsAssignableFrom(typeof(TEntity)))
            //{
            //    builder.HasQueryFilter(a => EF.Property<string>(a, "TenantId") == "");
            //    builder.HasQueryFilter(a => !EF.Property<bool>(a, "IsDeleted"));
            //}

            // 
            ModelBuilderHelper.AppContextConfigSoftDeletedAndTenantQueryFilter( modelBuilder, _applicationContext);

        }





        public override int SaveChanges()
        {
            //统一在数据库上下文的操作前，触发缓存实体的数据清空。
            if (_publisher != null)
            {
                this.ChangeTracker.Entries().Where(a =>
                (a.State == EntityState.Added || a.State == EntityState.Modified || a.State == EntityState.Deleted) 
                && Attribute.IsDefined(a.Entity.GetType(), typeof(EnableEntityCacheAttribute)))
                    .Select(a => a.Entity.GetType().Name).Distinct().ToList()
                    .ForEach(entityName =>
                {
                    _publisher.Publish(EntityCacheManager.EntityCacheEventName, new EntityChangeEvent { EntityName = entityName });
                });
            }

            return base.SaveChanges();
        }

        // 不用要SeedData进行数据初始化，此方法会在每次migration时删除和创建数据
    }
}
