using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Snail.Common;
using Snail.Core;
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
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="dtos"></param>
        /// <param name="addFunc"></param>
        /// <param name="userId"></param>
        public static void AddList<TEntity, TDto, TKey>(this DbSet<TEntity> entities, List<TDto> dtos, Func<TDto, TEntity> addFunc, TKey userId)
           where TEntity : class, IIdField<TKey>
         where TDto : class, IIdField<TKey>
        {
            dtos.ForEach(dto =>
            {
                Add(entities, dto, addFunc, userId);
            });
        }

        /// <summary>
        /// 增加多个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="dtos"></param>
        /// <param name="mapper"></param>
        /// <param name="userId"></param>
        public static void AddList<TEntity, TDto, TKey>(this DbSet<TEntity> entities, List<TDto> dtos, IMapper mapper, TKey userId)
           where TEntity : class, IIdField<TKey>
         where TDto : class, IIdField<TKey>
        {
            dtos.ForEach(dto =>
            {
                Add(entities, dto, mapper, userId);
            });
        }


        #endregion
        #region add
        /// <summary>
        /// 增加单个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="dto"></param>
        /// <param name="addFunc"></param>
        /// <param name="userId"></param>
        public static void Add<TEntity, TDto, TKey>(this DbSet<TEntity> entities, TDto dto, Func<TDto, TEntity> addFunc, TKey userId)
          where TEntity : class, IIdField<TKey>
        where TDto : class, IIdField<TKey>
        {
            var now = DateTime.Now;
            var entity = addFunc(dto);
            if (string.IsNullOrEmpty(entity.Id?.ToString()))
            {
                entity.Id = IdGenerator.Generate<TKey>();
            }
            if (entity is IAudit<TKey> auditEntity)
            {
                if (!string.IsNullOrEmpty(userId?.ToString()))
                {
                    auditEntity.Creater = userId;
                    auditEntity.Updater = userId;
                }
                auditEntity.CreateTime = now;
                auditEntity.UpdateTime = now;
            }
            entities.Add(entity);
        }

        /// <summary>
        /// 增加单个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="dto"></param>
        /// <param name="mapper"></param>
        /// <param name="userId"></param>
        public static void Add<TEntity, TDto, TKey>(this DbSet<TEntity> entities, TDto dto, IMapper mapper, TKey userId)
           where TEntity : class, IIdField<TKey>
         where TDto : class, IIdField<TKey>
        {
            Add(entities, dto, (dtoPara) => mapper.Map<TEntity>(dtoPara), userId);
        }
        #endregion

        #region addOrUpdate
        /// <summary>
        /// 增加或更新
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
        public static TEntity AddOrUpdate<TEntity, TDto, TKey>(this DbSet<TEntity> entities, TDto dto, Func<TDto, TEntity> addFunc, Action<TDto, TEntity> updateFunc, TKey userId, List<TEntity> existEntities = null)
        where TEntity : class, IIdField<TKey>
        where TDto : class, IIdField<TKey>
        {
            var now = DateTime.Now;
            var entity = existEntities == null ? entities.Find(dto.Id) : existEntities.FirstOrDefault(a => a.Id.Equals(dto.Id));
            if (entity == null)
            {
                //add
                entity = addFunc(dto);
                if (string.IsNullOrEmpty(entity.Id?.ToString()))
                {
                    entity.Id = IdGenerator.Generate<TKey>();
                }
                entities.Add(entity);
            }
            else
            {
                //update
                updateFunc(dto, entity);
            }

            if (entity is IAudit<TKey> auditEntity)
            {
                if (!string.IsNullOrEmpty(userId?.ToString()))
                {
                    auditEntity.Creater = userId;
                    auditEntity.Updater = userId;
                }
                auditEntity.CreateTime = now;
                auditEntity.UpdateTime = now;
            }
            return entity;
        }

        /// <summary>
        /// 增加或更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="dto"></param>
        /// <param name="mapper"></param>
        /// <param name="userId"></param>
        public static TEntity AddOrUpdate<TEntity, TDto, TKey>(this DbSet<TEntity> entities, TDto dto, IMapper mapper, TKey userId, List<TEntity> existEntities = null)
        where TEntity : class, IIdField<TKey>
        where TDto : class, IIdField<TKey>
        {
            return AddOrUpdate(entities, 
                dto, 
                dtoPara => mapper.Map<TEntity>(dtoPara), 
                (dtoPara, entity) => mapper.Map<TDto, TEntity>(dtoPara, entity),
                userId,
                existEntities);
        }

        #endregion

        #region addOrUpdateList

        /// <summary>
        /// 增加或更新多个
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="existEntities">已经存在的实体</param>
        /// <param name="dtos">要</param>
        /// <param name="addFunc"></param>
        /// <param name="updateFunc"></param>
        /// <param name="userId"></param>
        public static void AddOrUpdateList<TEntity, TDto, TKey>(this DbSet<TEntity> entities, List<TEntity> existEntities, List<TDto> dtos, Func<TDto, TEntity> addFunc, Action<TDto, TEntity> updateFunc, TKey userId)
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
            RemoveByIds<TEntity, TKey>(entities, removeIds, existEntities);


            // 增加或更新
            foreach (var dto in dtos.Where(a => !removeIds.Contains(a.Id)))
            {
                AddOrUpdate(entities, dto, addFunc, updateFunc, userId);
            }
        }


        /// <summary>
        /// 增加或更新多个
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="existsEntities"></param>
        /// <param name="dtos"></param>
        /// <param name="mapper"></param>
        /// <param name="userId"></param>
        public static void AddOrUpdateList<TEntity, TDto, TKey>(this DbSet<TEntity> entities, List<TEntity> existsEntities, List<TDto> dtos, IMapper mapper, TKey userId)
           where TEntity : class, IIdField<TKey>
           where TDto : class, IIdField<TKey>
        {
            AddOrUpdateList(entities, existsEntities, dtos, dto => mapper.Map<TEntity>(dto), (dto, entity) => mapper.Map<TDto, TEntity>(dto, entity), userId);
        }

        #endregion

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="ids"></param>
        /// <param name="existEntities"></param>
        public static void RemoveByIds<TEntity, TKey>(this DbSet<TEntity> entities, List<TKey> ids, List<TEntity> existEntities = null)
           where TEntity : class, IIdField<TKey>
        {
            foreach (var id in ids)
            {
                var entity = existEntities == null ? entities.Find(id) : existEntities.FirstOrDefault(a => a.Id.Equals(id));
                if (entity != null)
                {
                    if (entity is ISoftDelete entitySoftDeleteEntity)
                    {
                        entitySoftDeleteEntity.IsDeleted = true;
                        if (entity is IAudit<TKey> auditEntity)
                        {
                            auditEntity.CreateTime = DateTime.Now;
                            auditEntity.UpdateTime = DateTime.Now;
                        }
                    }
                    else
                    {
                        entities.Remove(entity);
                    }
                }
            }
        }
    }
}
