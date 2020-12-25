using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Snail.Common;
using Snail.Core;
using Snail.Core.Enum;
using Snail.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Snail.EntityFrameworkCore
{
    /// <summary>
    /// DbSet的扩展帮助类方法
    /// </summary>
    public static class DbSetExtenssion
    {
        #region addList
        /// <summary>
        /// 增加多个实体
        /// 请在外部提交更改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="dtos"></param>
        /// <param name="addFunc"></param>
        /// <param name="userId"></param>
        public static void AddList<TEntity, TDto, TKey>(this DbSet<TEntity> entities, List<TDto> dtos, Func<TDto, TEntity> addFunc, TKey userId, TKey tenantId = default)
           where TEntity : class, IIdField<TKey>
        {
            dtos.ForEach(dto =>
            {
                AddInternal(entities, dto, addFunc, userId, tenantId);
            });
        }

        /// <summary>
        /// 增加多个实体
        /// 请在外部提交更改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="dtos"></param>
        /// <param name="mapper"></param>
        /// <param name="userId"></param>
        public static void AddList<TEntity, TDto, TKey>(this DbSet<TEntity> entities, List<TDto> dtos, IMapper mapper, TKey userId, TKey tenantId = default)
           where TEntity : class, IIdField<TKey>
         where TDto : class, IIdField<TKey>
        {
            dtos.ForEach(dto =>
            {
                Add(entities, dto, mapper, userId, tenantId);
            });
        }

        /// <summary>
        /// 增加多个实体,用EasyMap对实体进行映射
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="dtos"></param>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        public static void AddList<TEntity, TDto, TKey>(this DbSet<TEntity> entities, List<TDto> dtos, TKey userId, TKey tenantId = default)
           where TEntity : class, IIdField<TKey>, new()
        {
            dtos.ForEach(dto =>
            {
                Add(entities, dto, userId, tenantId);
            });
        }

        #endregion
        #region add
        /// <summary>
        /// 增加单个实体，实体映射由用户自定义，所有增加单个实体的最终实现
        /// 请在外部提交更改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="dto"></param>
        /// <param name="addFunc"></param>
        /// <param name="userId"></param>
        public static void AddInternal<TEntity, TDto, TKey>(this DbSet<TEntity> entities, TDto dto, Func<TDto, TEntity> addFunc, TKey userId, TKey tenantId = default)
          where TEntity : class, IIdField<TKey>
        {
            var entity = addFunc(dto);
            if (string.IsNullOrEmpty(entity.Id?.ToString()))
            {
                entity.Id = IdGenerator.Generate<TKey>();
            }
            UpdateEntityCommonField(entity, EEntityOperType.Add, userId, tenantId);
            CheckEntityTenantOper(EEntityOperType.Delete, entity, userId, tenantId);
            entities.Add(entity);
        }

        /// <summary>
        /// 增加单个实体，映射实体只automapper处理
        /// 请在外部提交更改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="dto"></param>
        /// <param name="mapper"></param>
        /// <param name="userId"></param>
        public static void Add<TEntity, TDto, TKey>(this DbSet<TEntity> entities, TDto dto, IMapper mapper, TKey userId, TKey tenantId = default)
           where TEntity : class, IIdField<TKey>
        {
            AddInternal(entities, dto, (dtoPara) => mapper.Map<TEntity>(dtoPara), userId, tenantId);
        }

        /// <summary>
        /// 增加单个实体，会用EasyMap做实体映射
        /// 请在外部提交更改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        public static void Add<TEntity, TDto, TKey>(this DbSet<TEntity> entities, TDto dto, TKey userId, TKey tenantId = default)
           where TEntity : class, IIdField<TKey>, new()
        {
            AddInternal(entities, dto, sourceDto => EasyMap.MapToNew<TEntity>(sourceDto), userId, tenantId);
        }
        #endregion

        #region addOrUpdate
        /// <summary>
        /// 增加或更新的最终实现
        /// 请在外部提交更改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="dto">要增加或更新的对象</param>
        /// <param name="addFunc"></param>
        /// <param name="updateFunc"></param>
        /// <param name="userId"></param>
        /// <param name="existEntities"></param>
        public static TEntity AddOrUpdateInternal<TEntity, TDto, TKey>(this DbSet<TEntity> entities, TDto dto, Func<TDto, TEntity> addFunc, Action<TDto, TEntity> updateFunc, TKey userId, TKey tenantId = default, List<TEntity> existEntities = null)
        where TEntity : class, IIdField<TKey>
        where TDto : class, IIdField<TKey>
        {
            var entity = existEntities == null ? entities.Find(dto.Id) : existEntities.FirstOrDefault(a => a.Id.Equals(dto.Id));
            if (entity == null)
            {
                AddInternal(entities, dto, addFunc, userId, tenantId);
            }
            else
            {
                //update
                CheckEntityTenantOper(EEntityOperType.Delete, entity, userId, tenantId);
                updateFunc(dto, entity);
                UpdateEntityCommonField(entity, EEntityOperType.Update, userId, tenantId);
            }


            return entity;
        }

        /// <summary>
        /// 增加或更新
        /// 请在外部提交更改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="dto"></param>
        /// <param name="mapper"></param>
        /// <param name="userId"></param>
        public static TEntity AddOrUpdate<TEntity, TDto, TKey>(this DbSet<TEntity> entities, TDto dto, IMapper mapper, TKey userId, TKey tenantId = default, List<TEntity> existEntities = null)
        where TEntity : class, IIdField<TKey>
        where TDto : class, IIdField<TKey>
        {
            return AddOrUpdateInternal(entities,
                dto,
                dtoPara => mapper.Map<TEntity>(dtoPara),
                (dtoPara, entity) => mapper.Map<TDto, TEntity>(dtoPara, entity),
                userId,
                tenantId,
                existEntities);
        }

        public static TEntity AddOrUpdate<TEntity, TDto, TKey>(this DbSet<TEntity> entities, TDto dto, TKey userId, TKey tenantId = default, List<TEntity> existEntities = null)
       where TEntity : class, IIdField<TKey>, new()
       where TDto : class, IIdField<TKey>
        {
            return AddOrUpdateInternal(entities,
                dto,
                sourceDto => EasyMap.MapToNew<TEntity>(sourceDto),
                (sourceDto, entityDto) => EasyMap.Map(sourceDto.GetType(), entityDto.GetType(), sourceDto, entityDto, null),
                userId,
                tenantId,
                existEntities);
        }

        #endregion

        #region addOrUpdateList

        /// <summary>
        /// 实体列表的差异更新，包含增加、删除、更新
        /// 请在外部提交更改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities">dbset</param>
        /// <param name="existEntities">已经存在的实体列表</param>
        /// <param name="dtos">最终要更新为的实体列表</param>
        /// <param name="addFunc"></param>
        /// <param name="updateFunc"></param>
        /// <param name="userId"></param>
        public static void AddOrUpdateListInternal<TEntity, TDto, TKey>(this DbSet<TEntity> entities, List<TEntity> existEntities, List<TDto> dtos, Func<TDto, TEntity> addFunc, Action<TDto, TEntity> updateFunc, TKey userId, TKey tenantId = default)
           where TEntity : class, IIdField<TKey>
           where TDto : class, IIdField<TKey>
        {
            if (dtos == null || dtos.Count == 0)
            {
                return;
            }

            var dtoIds = dtos.Select(a => a.Id).ToList();
            // 要删除的ids
            var removeIds = existEntities?.Select(a => a.Id).Except(dtoIds).ToList() ?? new List<TKey>();
            // 更新ids删除对象
            RemoveByIds<TEntity, TKey>(entities, removeIds, userId,tenantId);


            // 增加或更新
            foreach (var dto in dtos.Where(a => !removeIds.Contains(a.Id)))
            {
                AddOrUpdateInternal(entities, dto, addFunc, updateFunc, userId, tenantId);
            }
        }


        /// <summary>
        /// 实体列表的差异更新，包含增加、删除、更新
        /// 请在外部提交更改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="existsEntities"></param>
        /// <param name="dtos"></param>
        /// <param name="mapper"></param>
        /// <param name="userId"></param>
        public static void AddOrUpdateList<TEntity, TDto, TKey>(this DbSet<TEntity> entities, List<TEntity> existsEntities, List<TDto> dtos, IMapper mapper, TKey userId, TKey tenantId = default)
           where TEntity : class, IIdField<TKey>
           where TDto : class, IIdField<TKey>
        {
            AddOrUpdateListInternal(entities, existsEntities, dtos, dto => mapper.Map<TEntity>(dto), (dto, entity) => mapper.Map<TDto, TEntity>(dto, entity), userId, tenantId);
        }

        #endregion

        #region 删除实体
        /// <summary>
        /// 删除实体
        /// 请在外部提交更改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="ids"></param>
        /// <param name="existEntities"></param>
        public static void RemoveByIds<TEntity, TKey>(this DbSet<TEntity> entities, List<TKey> ids, TKey userId, TKey tenantId = default)
           where TEntity : class, IIdField<TKey>
        {
            if (ids == null || ids.Count == 0)
            {
                return;
            }
            foreach (var id in ids)
            {
                RemoveById(entities, id, userId, tenantId);
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities">dbset</param>
        /// <param name="id">id</param>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        /// <param name="existEntities">从existEntities里删除而不是从dbset里删除</param>
        public static void RemoveById<TEntity, TKey>(this DbSet<TEntity> entities, TKey id, TKey userId, TKey tenantId = default)
           where TEntity : class, IIdField<TKey>
        {
            var entity = entities.Find(id);
           
            if (entity != null)
            {
                CheckEntityTenantOper(EEntityOperType.Delete, entity, userId, tenantId);
                if (entity is ISoftDelete entitySoftDeleteEntity)
                {
                    UpdateEntityCommonField(entity, EEntityOperType.Delete, userId, default);
                }
                else
                {
                    entities.Remove(entity);
                }
            }

        }
        #endregion

        /// <summary>
        /// 根据实体的操作类型，修改IAudit,ISoftDelete,ITenant等字段
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entity"></param>
        /// <param name="operType"></param>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        public static void UpdateEntityCommonField<TKey>(object entity, EEntityOperType operType, TKey userId, TKey tenantId = default)
        {
            if (entity is IAudit<TKey> auditEntity)
            {
                if (operType == EEntityOperType.Add)
                {
                    auditEntity.Creater = userId;
                    auditEntity.CreateTime = DateTime.Now;
                    if (entity is ITenant<TKey> tenantEntity)
                    {
                        tenantEntity.TenantId = tenantId;
                    }
                }
                auditEntity.Updater = userId;
                auditEntity.UpdateTime = DateTime.Now;
            }

            if (entity is ISoftDelete entitySoftDeleteEntity && operType == EEntityOperType.Delete)
            {
                entitySoftDeleteEntity.IsDeleted = true;
            }
        }

        public static void CheckEntityTenantOper<TEntity,TKey>(EEntityOperType operType,TEntity entity,TKey userId,TKey tenantId)
           where TEntity : class, IIdField<TKey>
        {
            // 跨租户实体操作限制
            if (tenantId != null && TenantHelper.HasTenant(entity, out TKey tenantIdTmp) && !tenantId.Equals(tenantIdTmp))
            {
                throw new InvalidOperationException($"不允许跨租户操作数据，操作类型:{operType}，表:{typeof(TEntity).Name}，实体id:{entity.Id}，操作人:{userId}，操作者租户:{tenantId}");
            }
        }
       
    }
}
