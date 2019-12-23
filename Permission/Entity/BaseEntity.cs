using Snail.Core.Entity;
using System;

namespace Snail.Entity
{
    /// <summary>
    /// 实体的公共属性，如ID，创建时间，最后一次更新时间，是否有效
    /// </summary>
    public class BaseEntity<TKey> : IEntityId<TKey>, IEntityAudit<TKey>, IEntitySoftDelete
    {
        ///// <summary>
        ///// 主键
        ///// </summary>
        //[Key]
        //public int Id { get; set; }
        //public DateTime CreateTime { get; set; }
        //public DateTime UpdateTime { get; set; }=DateTime.Now;
        //public int IsValid { get; set; } = 1;
        public TKey Id { get;set;}
        public TKey Creater { get;set;}
        public DateTime CreateTime { get;set;}
        public TKey Updater { get;set;}
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
    public class EntityId<TKey> : IEntityId<TKey>
    {
        public TKey Id { get; set; }
    }
}
