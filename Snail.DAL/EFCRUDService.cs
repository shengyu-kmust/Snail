using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Snail.Common;
using Snail.Core;
using Snail.Core.Entity;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 
/// </summary>
/// 
// todo：实现createrId的赋值
namespace Snail.DAL
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TSource">查询的全量字段返回类型，具体的返回类型TResult会从Source对类映射出来</typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class EFCRUDService<TEntity,TSource,TKey> : ICRUDService<TEntity, TKey> where TEntity : class, IEntityId<TKey>
    {
        public DbContext db;
        public IMapper _mapper;
        public DbSet<TEntity> entities;
        public IQueryable<TSource> QuerySource { get; set; }
        public EFCRUDService(DbContext db,IMapper mapper)
        {
            this.db = db;
            this._mapper = mapper;
            this.entities = db.Set<TEntity>();
        }

        public abstract TKey GetCurrentUserId();
        public abstract void InitQuerySource();

        public virtual TEntity Add<TSaveDto>(TSaveDto saveDto) where TSaveDto : IIdField<TKey>
        {
            if (saveDto.Id == default || saveDto.Id==null)
            {
                saveDto.Id = IdGenerator.Generate<TKey>();
            }
            var entity = _mapper.Map<TEntity>(saveDto);
            if (entity is IEntityAudit<TKey> entityAudit)
            {
                entityAudit.UpdateTime = DateTime.Now;
                entityAudit.CreateTime = DateTime.Now;
                if (GetCurrentUserId()!=null)
                {
                    entityAudit.CreaterId = GetCurrentUserId();
                    entityAudit.UpdaterId = GetCurrentUserId();
                }
            }
            entities.Add(entity);
            db.SaveChanges();
            return entities.Find(saveDto.Id);
        }

        public virtual void Delete(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException();
            }
            var entity = entities.Find(id);
            if (entity==null)
            {
                throw new BusinessException($"您要删除的对象不存在，id为{id}");
            }
            if (entity is IEntityAudit<TKey> entityAudit)
            {
                entityAudit.UpdateTime = DateTime.Now;
                if (GetCurrentUserId() != null)
                {
                    entityAudit.UpdaterId = GetCurrentUserId();
                }
            }
            if (entity is IEntitySoftDelete entitySoftDelete)
            {
                entitySoftDelete.IsDeleted = true;
            }
            else
            {
                entities.Remove(entity);
            }
            db.SaveChanges();
        }

        public virtual List<TResult> Query<TResult, TQueryDto>(TQueryDto queryDto) where TResult : class
        {
            return QueryInternal<TResult, TQueryDto>(queryDto).ToList();
        }

        public virtual IPageResult<TResult> QueryPage<TResult, TQueryDto>(TQueryDto queryDto)
            where TResult : class
            where TQueryDto : IPagination
        {
            var resultQuery = QueryInternal<TResult, TQueryDto>(queryDto);
            return resultQuery.ToPageList(queryDto);
        }
        private IQueryable<TResult> QueryInternal<TResult, TQueryDto>(TQueryDto queryDto)
        {
            IQueryable<TSource> query = QuerySource;
            // 查询条件
            if (queryDto is IPredicateConvert<TSource> predicateConvert)
            {
                var predicate = predicateConvert.GetExpression();
                query = query.Where(predicate);
            }
            else
            {
                var predicate = SimpleEntityExpressionGenerator.GenerateAndExpressionFromDto<TSource>(queryDto);
                query = query.Where(predicate);
            }
            IQueryable<TResult> result;
            // 结果映射
            if (queryDto is ISelectorBuilder<TSource, TResult> selectorBuilder)
            {
                result = query.Select(selectorBuilder.GetSelector());
            }
            else
            {
                result = _mapper.ProjectTo<TResult>(query);
            }
            return result;
        }

        public virtual TEntity Update<TSaveDto>(TSaveDto saveDto) where TSaveDto : IIdField<TKey>
        {
            if (saveDto.Id == default || saveDto.Id==null)
            {
                throw new Exception("修改时，必须转入id");
            }
            var entity = entities.Find(saveDto.Id);
            if (entity == null)
            {
                throw new Exception("要修改的实体不存在");
            }
            _mapper.Map(saveDto, entity, typeof(TSaveDto), typeof(TEntity));
            if (entity is IEntityAudit<TKey> entityAudit)
            {
                entityAudit.UpdateTime = DateTime.Now;
                entityAudit.CreateTime = DateTime.Now;
                if (GetCurrentUserId() != null)
                {
                    entityAudit.UpdaterId = GetCurrentUserId();
                }
            }
            db.SaveChanges();
            return entity;
        }
    }
}
