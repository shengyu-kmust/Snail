using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Snail.Common;
using Snail.Core.Attributes;
using Snail.Core.Entity;
using Snail.Core.Enum;
using Snail.Core.Interface;
using Snail.Core.Permission;
using Snail.Permission.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Snail.Common.Extenssions;
using System.Reflection;

namespace Snail.Permission
{
    public class DefaultPermissionStore : IPermissionStore
    {
        private DbContext _db;
        private IMemoryCache _memoryCache;
        private IApplicationContext _applicationContext;
        private string userCacheKey = $"DefaultPermissionStore_{nameof(userCacheKey)}", roleCacheKey = $"DefaultPermissionStore_{nameof(roleCacheKey)}", userRoleCacheKey = $"DefaultPermissionStore_{nameof(userRoleCacheKey)}", resourceCacheKey = $"DefaultPermissionStore_{nameof(resourceCacheKey)}", roleResourceCacheKey = $"DefaultPermissionStore_{nameof(roleResourceCacheKey)}";
        protected PermissionOptions _permissionOptions;

        public DefaultPermissionStore(DbContext db, IMemoryCache memoryCache, IOptionsMonitor<PermissionOptions> permissionOptions, IApplicationContext applicationContext)
        {
            _db = db;
            _memoryCache = memoryCache;
            _permissionOptions = permissionOptions.CurrentValue ?? new PermissionOptions();
            _applicationContext = applicationContext;
        }
        public virtual List<IResource> GetAllResource()
        {
            return _memoryCache.GetOrCreate(resourceCacheKey, a => _db.Set<Resource>().AsNoTracking().Select(i => (IResource)i).ToList());
        }

        public List<IRole> GetAllRole()
        {
            return _memoryCache.GetOrCreate(roleCacheKey, a => _db.Set<Role>().AsNoTracking().Select(i => (IRole)i).ToList());

        }

        public List<IRoleResource> GetAllRoleResource()
        {
            return _memoryCache.GetOrCreate(roleResourceCacheKey, a => _db.Set<RoleResource>().AsNoTracking().Select(i => (IRoleResource)i).ToList());

        }

        public List<IUser> GetAllUser()
        {
            return _memoryCache.GetOrCreate(userCacheKey, a => _db.Set<User>().AsNoTracking().Select(i => (IUser)i).ToList());

        }

        public List<IUserRole> GetAllUserRole()
        {
            return _memoryCache.GetOrCreate(userRoleCacheKey, a => _db.Set<UserRole>().AsNoTracking().Select(i => (IUserRole)i).ToList());

        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 经验证，cache里的值在remove后，之前已经从cache里获取的值不会删除
        /// </remarks>
        public void ReloadPemissionDatas()
        {
            _memoryCache.Remove(userCacheKey);
            _memoryCache.Remove(roleCacheKey);
            _memoryCache.Remove(userRoleCacheKey);
            _memoryCache.Remove(resourceCacheKey);
            _memoryCache.Remove(roleResourceCacheKey);
        }



        public void RemoveRole(string roleKey)
        {
            //角色真删除
            var roleEntity = _db.Set<Role>().Where(a => a.Id == roleKey);
            _db.Remove(roleEntity);
            _db.SaveChanges();
            _memoryCache.Remove(roleCacheKey);

        }


        public void RemoveUser(string userKey)
        {
            var userEntity = _db.Set<User>().FirstOrDefault(a => a.Id == userKey);
            if (userEntity != null)
            {
                userEntity.IsDeleted = true;
                _db.SaveChanges();
            }
            _memoryCache.Remove(userCacheKey);
        }


        public void RemoveResource(string resourceKey)
        {
            var resourceEntity = _db.Set<Resource>().FirstOrDefault(a => a.Id == resourceKey);
            if (resourceEntity != null)
            {
                _db.Remove(resourceEntity);//资源为真删
                _db.SaveChanges();
            }
            _memoryCache.Remove(resourceCacheKey);
        }
        /// <summary>
        /// 保存资源。会从资源id和资源code两字段考虑是新增还是修改
        /// </summary>
        /// <param name="resource"></param>
        public void SaveResource(IResource resource)
        {
            var resoureDto = resource as Resource;
            var resourceKey = resource.GetKey();
            var userId = _applicationContext.GetCurrentUserId();
            var resourceEntity = _db.Set<Resource>().FirstOrDefault(a => a.Id == resourceKey || a.Code == resource.GetResourceCode());
            if (resourceEntity == null)
            {
                //add
                var addDto = new Resource
                {
                    Creater = userId,
                    CreateTime = DateTime.Now,
                    IsDeleted = false,
                    Code = resource.GetResourceCode(),
                    Name = resource.GetName(),
                    ParentId = resoureDto.ParentId,
                    Id = string.IsNullOrEmpty(resoureDto.Id) ? IdGenerator.Generate<string>() : resoureDto.Id,
                    Updater = userId,
                    UpdateTime = DateTime.Now
                };
                _db.Add(addDto);
            }
            else
            {
                resourceEntity.Name = resource.GetName();
                resourceEntity.Code = resource.GetResourceCode();
                resourceEntity.ParentId = resoureDto.ParentId;
                resourceEntity.Updater = userId;
                resourceEntity.UpdateTime = DateTime.Now;
            }
            _db.SaveChanges();
            _memoryCache.Remove(resourceCacheKey);
        }
        public void SaveRole(IRole role)
        {
            var roleKey = role.GetKey();
            var userId = _applicationContext.GetCurrentUserId();
            if (string.IsNullOrEmpty(roleKey))
            {
                //add
                _db.Add(new Role
                {
                    Creater = userId,
                    CreateTime = DateTime.Now,
                    Id = IdGenerator.Generate<string>(),
                    IsDeleted = false,
                    Name = role.GetName(),
                    Updater = userId,
                    UpdateTime = DateTime.Now                    
                });
            }
            else
            {
                var roleEntity = _db.Set<Role>().FirstOrDefault(a => a.Id == roleKey);
                if (roleEntity != null)
                {
                    roleEntity.Name = role.GetName();
                    roleEntity.Updater = userId;
                    roleEntity.UpdateTime = DateTime.Now;
                }
            }
            _db.SaveChanges();
            _memoryCache.Remove(roleCacheKey);

        }

        public void SaveUser(IUser user)
        {
            var userDto = user as User;
            var userKey = user.GetKey();
            var userId = _applicationContext.GetCurrentUserId();
            if (string.IsNullOrEmpty(userKey))
            {
                //add
                _db.Add(new User
                {
                    Creater = userId,
                    CreateTime = DateTime.Now,
                    Id = IdGenerator.Generate<string>(),
                    IsDeleted = false,
                    Name = user.GetName(),
                    Updater = userId,
                    UpdateTime = DateTime.Now,
                    Account= userDto?.Account,
                    Email=userDto?.Email,
                    Gender=userDto?.Gender??EGender.Male,
                    Phone=userDto?.Phone,
                    Pwd=userDto?.Pwd
                });
            }
            else
            {
                var roleEntity = _db.Set<User>().FirstOrDefault(a => a.Id == userKey);
                if (roleEntity != null)
                {
                    if (!string.IsNullOrEmpty(user.GetName()))
                    {
                        roleEntity.Name = user.GetName();
                    }
                    if (!string.IsNullOrEmpty(user.GetAccount()))
                    {
                        roleEntity.Account = user.GetAccount();
                    }
                    if (!string.IsNullOrEmpty(userDto.Email))
                    {
                        roleEntity.Email = userDto.Email;
                    }
                    if (!string.IsNullOrEmpty(userDto.Phone))
                    {
                        roleEntity.Phone = userDto.Phone;
                    }
                    if (!string.IsNullOrEmpty(userDto.Pwd))
                    {
                        roleEntity.Pwd = userDto.Pwd;
                    }
                    roleEntity.Gender = userDto.Gender;
                    roleEntity.Updater = userId;
                    roleEntity.UpdateTime = DateTime.Now;
                    
                }
            }
            _db.SaveChanges();
            _memoryCache.Remove(userCacheKey);
        }

        public void SetRoleResources(string roleKey, List<string> resourceKeys)
        {
            var userId = _applicationContext.GetCurrentUserId();
            var allRoleResources = _db.Set<RoleResource>().AsNoTracking().Where(a => a.RoleId == roleKey).ToList();
            var allResource = _db.Set<Resource>().AsNoTracking().ToList();
            allRoleResources.Where(a => !resourceKeys.Contains(a.GetResourceKey())).ToList().ForEach(a =>
            {
                _db.Remove(a);
            });
            resourceKeys.Where(a => !allRoleResources.Select(i => i.GetResourceKey()).Contains(a)).ToList().ForEach(resourceKey =>
            {
                if (allResource.Any(a=>a.Id==resourceKey))
                {
                    _db.Add(new RoleResource
                    {
                        Id = IdGenerator.Generate<string>(),
                        Creater = userId,
                        CreateTime = DateTime.Now,
                        IsDeleted = false,
                        ResourceId = resourceKey,
                        RoleId = roleKey,
                        Updater = userId,
                        UpdateTime = DateTime.Now
                    });
                }
            });
            _db.SaveChanges();
            _memoryCache.Remove(roleResourceCacheKey);
        }

        public void SetUserRoles(string userKey, List<string> roleKeys)
        {
            var userId = _applicationContext.GetCurrentUserId();
            var allUserRoles = _db.Set<UserRole>().AsNoTracking().Where(a => a.UserId == userKey).ToList();
            var allRole = _db.Set<Role>().AsNoTracking().ToList();
            allUserRoles.Where(a => !roleKeys.Contains(a.GetRoleKey())).ToList().ForEach(a =>
            {
                _db.Remove(a);
            });
            roleKeys.Where(a => !allUserRoles.Select(i => i.RoleId).Contains(a) && a.HasValue()).ToList().ForEach(roleKey =>
            {
                if (allRole.Any(a=>a.Id==roleKey))
                {
                    _db.Add(new UserRole
                    {
                        Id = IdGenerator.Generate<string>(),
                        Creater = userId,
                        CreateTime = DateTime.Now,
                        IsDeleted = false,
                        UserId = userKey,
                        RoleId = roleKey,
                        Updater = userId,
                        UpdateTime = DateTime.Now
                    });
                }
               
            });
            _db.SaveChanges();
            _memoryCache.Remove(userRoleCacheKey);
        }

    }
}
