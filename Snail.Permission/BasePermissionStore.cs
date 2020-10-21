using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Snail.Common;
using Snail.Core.Entity;
using Snail.Core.Interface;
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
    /// 
    public class BasePermissionStore<TDbContext, TUser, TRole, TUserRole, TResource, TRoleResource> : IPermissionStore
        where TDbContext : DbContext
        where TUser : class, IUser, new()
        where TRole : class, IRole, new()
        where TUserRole : class, IUserRole, new()
        where TResource : class, IResource, new()
        where TRoleResource : class, IRoleResource, new()
    {
        protected TDbContext _db;
        protected IMemoryCache _memoryCache;
        protected IApplicationContext _applicationContext;
        protected readonly string userCacheKey = $"DefaultPermissionStore_{nameof(userCacheKey)}";
        protected readonly string roleCacheKey = $"DefaultPermissionStore_{nameof(roleCacheKey)}";
        protected readonly string userRoleCacheKey = $"DefaultPermissionStore_{nameof(userRoleCacheKey)}";
        protected readonly string resourceCacheKey = $"DefaultPermissionStore_{nameof(resourceCacheKey)}";
        protected readonly string roleResourceCacheKey = $"DefaultPermissionStore_{nameof(roleResourceCacheKey)}";

        protected IOptionsMonitor<PermissionOptions> _permissionOptions;

        public BasePermissionStore(TDbContext db, IMemoryCache memoryCache, IOptionsMonitor<PermissionOptions> permissionOptions, IApplicationContext applicationContext)
        {
            _db = db;
            _memoryCache = memoryCache;
            _permissionOptions = permissionOptions;
            _applicationContext = applicationContext;
        }

        #region 查询数据
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
            return _memoryCache.GetOrCreate(userCacheKey, a => _db.Set<TUser>().AsNoTracking().Select(i => (IUser)i).ToList());

        }

        public virtual List<IUserRole> GetAllUserRole()
        {
            return _memoryCache.GetOrCreate(userRoleCacheKey, a => _db.Set<TUserRole>().AsNoTracking().Select(i => (IUserRole)i).ToList());

        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 经验证，cache里的值在remove后，之前已经从cache里获取的值不会删除
        /// </remarks>
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
            var roleEntity = GetAllRole().FirstOrDefault(a => a.GetKey() == roleKey) as TRole;
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
            var resourceKey = resource.GetKey();
            // todo 全表查询了
            var resourceEntity = _db.Set<TResource>().AsEnumerable().FirstOrDefault(a => a.GetKey() == resourceKey || a.GetResourceCode() == resource.GetResourceCode());
            if (resourceEntity == null)
            {
                //add
                var addDto = new TResource();
                EasyMap.Map(resource.GetType(), typeof(TResource), resource, addDto,null);
                _db.Add(addDto);
            }
            else
            {
                EasyMap.Map(resource.GetType(), typeof(TResource), resource, resourceEntity, null);
            }
            SetAuditSoftDelete(resourceEntity);
            _db.SaveChanges();
            _memoryCache.Remove(resourceCacheKey);
        }
        /// <summary>
        /// 保存资源。会从资源id和资源code两字段考虑是新增还是修改
        /// </summary>
        /// <param name="resource"></param>
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
                }
                else
                {
                    EasyMap.Map(resource.GetType(), typeof(TResource), resource, resourceEntity, null);
                }
                SetAuditSoftDelete(resourceEntity);
            });
        
            _db.SaveChanges();
            _memoryCache.Remove(resourceCacheKey);
        }



        public virtual void UpdateRoleEntityByDto(IRole entity, IRole dto, bool isAdd)
        {
            EasyMap.Map(dto.GetType(), entity.GetType(), dto, entity, null);
            SetAuditSoftDelete(entity);
        }
        public virtual void UpdateUserEntityByDto(IUser entity, IUser dto, bool isAdd)
        {
            EasyMap.Map(dto.GetType(), entity.GetType(), dto, entity, null);
            SetAuditSoftDelete(entity);
        }

        public virtual void SaveRole(IRole role)
        {
            var roleKey = role.GetKey();
            if (string.IsNullOrEmpty(roleKey))
            {
                var addRole = new TRole();
                UpdateRoleEntityByDto(addRole, role, true);
                _db.Add(addRole);
            }
            else
            {
                var editRole = _db.Set<TRole>().Find(role.GetKey());
                UpdateRoleEntityByDto(editRole, role, false);
            }
            _db.SaveChanges();
            _memoryCache.Remove(roleCacheKey);

        }

        public virtual void SaveUser(IUser user)
        {
            TUser saveUser;
            if (string.IsNullOrEmpty(user.GetKey()))
            {
                saveUser = new TUser();
                UpdateUserEntityByDto(saveUser, user, true);
                _db.Add(saveUser);
            }
            else
            {
                saveUser = _db.Set<TUser>().Find(user.GetKey());
                UpdateUserEntityByDto(saveUser, user, false);
            }
            saveUser.SetName(user.GetName());
            saveUser.SetAccount(user.GetAccount());
            saveUser.SetPassword(user.GetPassword());
            _db.SaveChanges();
            _memoryCache.Remove(userCacheKey);
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
                    SetAuditSoftDelete(addRoleResource);
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
            allUserRoles.Where(a => !roleKeys.Contains(a.GetRoleKey())).ToList().ForEach(a =>
            {
                _db.Remove(a);
            });
            roleKeys.Where(a => !allUserRoles.Select(i => i.GetRoleKey()).Contains(a) && !string.IsNullOrEmpty(a)).ToList().ForEach(roleKey =>
            {
                if (allRole.Any(a => a.GetKey() == roleKey))
                {
                    var addItem = new TUserRole();
                    SetAuditSoftDelete(addItem);
                    addItem.SetRoleKey(roleKey);
                    addItem.SetUserKey(userKey);
                    _db.Add(addItem);
                }

            });
            _db.SaveChanges();
            _memoryCache.Remove(userRoleCacheKey);
        }

        private void SetAuditSoftDelete(object obj)
        {
            var userId = _applicationContext.GetCurrentUserId();
            if (obj is IIdField<string> entityOfIdField && string.IsNullOrEmpty(entityOfIdField.Id))
            {
                entityOfIdField.Id = IdGenerator.Generate<string>();
            }
            if (obj is IAudit<string> entityOfAudit)
            {
                entityOfAudit.CreateTime = DateTime.Now;
                entityOfAudit.Updater = userId;
                entityOfAudit.Updater = userId;
                entityOfAudit.Creater = userId;
            }
        }
    }
}
