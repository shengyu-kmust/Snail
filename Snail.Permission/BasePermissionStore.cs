using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core.Enum;
using Snail.Core.Interface;
using Snail.Core.Utilities;
using Snail.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Snail.Core.Permission
{
    /// <summary>
    /// 权限系统的存储基类，todo GetUserKey会全表查询，需优化
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TUserRole"></typeparam>
    /// <typeparam name="TResource"></typeparam>
    /// <typeparam name="TRoleResource"></typeparam>
    /// <typeparam name="TKey">系统主键的类型，如string,int,guid等</typeparam>
    /// 
    public class BasePermissionStore<TDbContext, TUser, TRole, TUserRole, TResource, TRoleResource, TKey> : IPermissionStore
        where TDbContext : DbContext
        where TUser : class, IUser, IIdField<TKey>, new()
        where TRole : class, IRole, IIdField<TKey>, new()
        where TUserRole : class, IUserRole, IIdField<TKey>, new()
        where TResource : class, IResource, IIdField<TKey>, new()
        where TRoleResource : class, IRoleResource, IIdField<TKey>, new()

    {
        /// <summary>
        /// 数据库
        /// </summary>
        protected TDbContext _db;
        /// <summary>
        /// 缓存
        /// </summary>
        protected IMemoryCache _memoryCache;
        /// <summary>
        /// 应用上下文
        /// </summary>
        protected IApplicationContext _applicationContext;
        /// <summary>
        /// 所有用户缓存的key
        /// </summary>
        protected readonly string userCacheKey = $"DefaultPermissionStore_{nameof(userCacheKey)}";
        /// <summary>
        /// 所有用户角色缓存的key
        /// </summary>
        protected readonly string roleCacheKey = $"DefaultPermissionStore_{nameof(roleCacheKey)}";
        /// <summary>
        /// 所有角色的key
        /// </summary>
        protected readonly string userRoleCacheKey = $"DefaultPermissionStore_{nameof(userRoleCacheKey)}";
        /// <summary>
        /// 所有资源的key
        /// </summary>
        protected readonly string resourceCacheKey = $"DefaultPermissionStore_{nameof(resourceCacheKey)}";
        /// <summary>
        /// 所有角色资源对应关系的key
        /// </summary>
        protected readonly string roleResourceCacheKey = $"DefaultPermissionStore_{nameof(roleResourceCacheKey)}";
        /// <summary>
        /// 是否为多租户系统
        /// </summary>
        private bool? _isTenant;

        /// <summary>
        /// 权限组件配置
        /// </summary>
        protected IOptionsMonitor<PermissionOptions> _permissionOptions;

        /// <summary>
        /// BasePermissionStore
        /// </summary>
        /// <param name="db"></param>
        /// <param name="memoryCache"></param>
        /// <param name="permissionOptions"></param>
        /// <param name="applicationContext"></param>
        public BasePermissionStore(TDbContext db, IMemoryCache memoryCache, IOptionsMonitor<PermissionOptions> permissionOptions, IApplicationContext applicationContext)
        {
            _db = db;
            _memoryCache = memoryCache;
            _permissionOptions = permissionOptions;
            _applicationContext = applicationContext;
        }

        #region 查询权限基础表数据，用缓存，缓存所有的基础表数据
        public virtual List<IResource> GetAllResource()
        {
            return _memoryCache.GetOrCreate(resourceCacheKey, a => _db.Set<TResource>().AsNoTracking().Select(i => (IResource)i).ToList());
        }

        public virtual List<IRole> GetAllRole()
        {
            return _memoryCache.GetOrCreate(roleCacheKey, a => _db.Set<TRole>().AsNoTracking().Select(i => (IRole)i).ToList());

        }

        public virtual List<IRoleResource> GetAllRoleResource()
        {
            return _memoryCache.GetOrCreate(roleResourceCacheKey, a => _db.Set<TRoleResource>().AsNoTracking().Select(i => (IRoleResource)i).ToList());

        }

        public virtual List<IUser> GetAllUser()
        {

            return _memoryCache.GetOrCreate(userCacheKey,
                a => _db.Set<TUser>().AsNoTracking().Select(i => (IUser)i).ToList());

        }



        public virtual List<IUserRole> GetAllUserRole()
        {
            return _memoryCache.GetOrCreate(userRoleCacheKey, a => _db.Set<TUserRole>().AsNoTracking().Select(i => (IUserRole)i).ToList());

        }


        /// <summary>
        /// 重新加载获取的基础表数据
        /// </summary>
        public virtual void ReloadPemissionDatas()
        {
            _memoryCache.Remove(userCacheKey);
            _memoryCache.Remove(roleCacheKey);
            _memoryCache.Remove(userRoleCacheKey);
            _memoryCache.Remove(resourceCacheKey);
            _memoryCache.Remove(roleResourceCacheKey);
        }
        #endregion


        public virtual void RemoveRole(string roleKey)
        {
            // 删除角色为真删除，不保留历史的角色和角色资源关系
            var roleEntity = GetAllRole().FirstOrDefault(a => a.GetKey() == roleKey) as TRole;
            TenantHelper.CheckEntityTenantOper(EEntityOperType.Delete, roleEntity, CurrentUserId, CurrentTenantId);
            if (roleEntity != null)
            {
                _db.Set<TRole>().Remove(roleEntity);
            }
            //roleUser
            _db.Set<TUserRole>().AsEnumerable().Where(a => a.GetRoleKey() == roleKey).ToList().ForEach(userRole =>
            {
                _db.Remove(userRole);
            });
            //roleResource
            _db.Set<TRoleResource>().AsEnumerable().Where(a => a.GetRoleKey() == roleKey).ToList().ForEach(roleResource =>
            {
                _db.Remove(roleResource);
            });
            _memoryCache.Remove(roleCacheKey);

        }


        public virtual void RemoveUser(string userKey)
        {
            var userEntity = _db.Set<TUser>().Find(userKey);
            TenantHelper.CheckEntityTenantOper(EEntityOperType.Delete, userEntity, CurrentUserId, CurrentTenantId);
            if (userEntity is ISoftDelete entitySoftDeleteEntity)
            {
                entitySoftDeleteEntity.IsDeleted = true;
            }
            else
            {
                _db.Set<TUser>().Remove(userEntity);
            }

            //roleUser
            _db.Set<TUserRole>().AsEnumerable().Where(a => a.GetUserKey() == userKey).ToList().ForEach(userRole =>
            {
                _db.Remove(userRole);
            });

            _db.SaveChanges();
            _memoryCache.Remove(userCacheKey);
        }

        public virtual void RemoveResource(string resourceKey)
        {
            var resourceEntity = _db.Set<TResource>().Find(resourceKey);
            if (resourceEntity != null)
            {
                _db.Remove(resourceEntity);//资源为真删
            }

            //roleResource
            _db.Set<TRoleResource>().AsEnumerable().Where(a => a.GetResourceKey() == resourceKey).ToList().ForEach(roleResource =>
            {
                _db.Remove(roleResource);
            });
            _db.SaveChanges();

            _memoryCache.Remove(resourceCacheKey);
        }

        /// <summary>
        /// 保存资源。会从资源id和资源code两字段考虑是新增还是修改
        /// </summary>
        /// <param name="resource"></param>
        public virtual void SaveResource(IResource resource)
        {
            var resourceTmp = EasyMap.MapToNew<TResource>(resource);
            var resourceEntity = _db.Set<TResource>().AddOrUpdate(resourceTmp, CurrentUserId, default, null);
            resourceEntity.SetName(resourceTmp.GetName());
            resourceEntity.SetParentKey(resourceTmp.GetParentKey());
            _db.SaveChanges();
            _memoryCache.Remove(resourceCacheKey);
        }
        /// <summary>
        /// 保存资源。会从资源id和资源code两字段考虑是新增还是修改，无删除
        /// </summary>
        /// <param name="resources"></param>
        public virtual void SaveResources(List<IResource> resources)
        {
            var allResources = _db.Set<TResource>().ToList();
            resources.ForEach(resource =>
            {
                var resourceKey = resource.GetKey();
                var resourceEntity = allResources.FirstOrDefault(a => a.GetKey() == resourceKey || a.GetResourceCode() == resource.GetResourceCode());

                if (resourceEntity == null)
                {
                    //add
                    var addDto = new TResource();
                    EasyMap.Map(resource.GetType(), typeof(TResource), resource, addDto, null);
                    _db.Add(addDto);
                    DbSetExtenssion.UpdateEntityCommonField(resourceEntity, EEntityOperType.Add, CurrentUserId, CurrentTenantId);
                }
                else
                {
                    EasyMap.Map(resource.GetType(), typeof(TResource), resource, resourceEntity, null);
                    DbSetExtenssion.UpdateEntityCommonField(resourceEntity, EEntityOperType.Update, CurrentUserId, CurrentTenantId);
                }
            });

            _db.SaveChanges();
            _memoryCache.Remove(resourceCacheKey);
        }
        public virtual void SaveRole(IRole role)
        {
            lock (Locker.GetLocker($"BasePermissionStore_SaveRole"))
            {
                var roleTmp = EasyMap.MapToNew<TRole>(role);
                _db.Set<TRole>().AddOrUpdate(roleTmp, CurrentUserId, CurrentTenantId, null);
                _db.SaveChanges();
                _memoryCache.Remove(roleCacheKey);
            }
        }

        public virtual void SaveUser(IUser user)
        {
            lock (Locker.GetLocker($"BasePermissionStore_SaveUser"))
            {
                // 账号不能重复.如果是多租户,同一租户里的账号不能重复.
                var allUser = GetAllUser();
                var existAccountUser = allUser.FirstOrDefault(a => a.GetAccount() == user.GetAccount());
                if (existAccountUser != null && existAccountUser.GetKey() != user.GetKey())
                {
                    throw new BusinessException($"已经存在账号为{existAccountUser.GetAccount()}的用户");
                }
                var userTmp = EasyMap.MapToNew<TUser>(user);
                var userEntity = _db.Set<TUser>().AddOrUpdate(userTmp, CurrentUserId, CurrentTenantId, null);
                userEntity.SetName(user.GetName());
                userEntity.SetAccount(user.GetAccount());
                userEntity.SetPassword(user.GetPassword());
                _db.SaveChanges();
                _memoryCache.Remove(userCacheKey);
            }
        }

        public virtual void SetRoleResources(string roleKey, List<string> resourceKeys)
        {
            var allRoleResources = _db.Set<TRoleResource>().AsNoTracking().ToList().Where(a => a.GetRoleKey() == roleKey).ToList(); // 这个是否为全表查询
            var allResources = _db.Set<TResource>().AsNoTracking().ToList();

            // 删除角色权限
            allRoleResources.Where(a => !resourceKeys.Contains(a.GetResourceKey())).ToList().ForEach(a =>
            {
                _db.Remove(a);
            });

            // 增加角色权限
            resourceKeys.Where(a => !allRoleResources.Select(i => i.GetResourceKey()).Contains(a)).ToList().ForEach(resourceKey =>
            {
                if (allResources.Any(a => a.GetKey() == resourceKey))
                {
                    var addRoleResource = new TRoleResource();
                    DbSetExtenssion.UpdateEntityCommonField(addRoleResource, EEntityOperType.Update, CurrentUserId, CurrentTenantId);
                    addRoleResource.SetRoleKey(roleKey);
                    addRoleResource.SetResourceKey(resourceKey);
                    _db.Add(addRoleResource);
                }
            });
            _db.SaveChanges();
            _memoryCache.Remove(roleResourceCacheKey);
        }

        public virtual void SetUserRoles(string userKey, List<string> roleKeys)
        {
            // todo 全表查了
            var allUserRoles = _db.Set<TUserRole>().AsEnumerable().Where(a => a.GetUserKey() == userKey).ToList();
            var allRole = _db.Set<TRole>().AsNoTracking().ToList();

            // 删除授权
            allUserRoles.Where(a => !roleKeys.Contains(a.GetRoleKey())).ToList().ForEach(a =>
            {
                _db.Remove(a);
            });

            //增加授权
            roleKeys.Where(a => !allUserRoles.Select(i => i.GetRoleKey()).Contains(a) && !string.IsNullOrEmpty(a)).ToList().ForEach(roleKey =>
            {
                if (allRole.Any(a => a.GetKey() == roleKey))
                {
                    var addItem = new TUserRole();
                    DbSetExtenssion.UpdateEntityCommonField(addItem, EEntityOperType.Update, CurrentUserId, CurrentTenantId);
                    addItem.SetRoleKey(roleKey);
                    addItem.SetUserKey(userKey);
                    _db.Add(addItem);
                }

            });
            _db.SaveChanges();
            _memoryCache.Remove(userRoleCacheKey);
        }

        //private void SetAuditSoftDeleteTenant(object obj)
        //{
        //    var userId = _applicationContext.GetCurrentUserId();
        //    if (obj is IIdField<string> entityOfIdField && string.IsNullOrEmpty(entityOfIdField.Id))
        //    {
        //        entityOfIdField.Id = IdGenerator.Generate<string>();
        //    }
        //    if (obj is IAudit<string> entityOfAudit)
        //    {
        //        entityOfAudit.CreateTime = DateTime.Now;
        //        entityOfAudit.Updater = userId;
        //        entityOfAudit.Updater = userId;
        //        entityOfAudit.Creater = userId;
        //    }
        //    if (obj is ITenant<string> tenantEntity)
        //    {
        //        tenantEntity.TenantId = _applicationContext.GetCurrnetTenantId();
        //    }
        //}

        public bool HasTenant(out string tenantId)
        {
            tenantId = string.Empty;
            if (!_isTenant.HasValue)
            {
                _isTenant = TenantHelper.HasTenant<TUser, TKey>(); 
            }
            if (_isTenant.Value)
            {
                tenantId = CurrentTenantId.ToString();
            }
            return _isTenant.Value;
        }

      
        public TKey CurrentUserId
        {
            get
            {
                return _applicationContext.GetCurrentUserId().ConvertTo<TKey>();
            }
        }
        public TKey CurrentTenantId
        {
            get
            {
                return _applicationContext.GetCurrnetTenantId().ConvertTo<TKey>();
            }
        }
    }
}
