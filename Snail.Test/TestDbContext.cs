using Microsoft.EntityFrameworkCore;
using Snail.Abstract.Entity;
using System;

namespace Snail.Test
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {
        }
    }

    public class Student : BaseEntity
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public Guid TeamId { get; set; }
    }


    public class Team
    {
        public string Name { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public class BaseEntity : IBaseEntity, IEntityId<Guid>, IEntityAudit<Guid>, IEntitySoftDelete
    {
        public bool IsDeleted { get; set; }
        public Guid CreaterId { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid UpdaterId { get; set; }
        public DateTime UpdateTime { get; set; }
        public Guid Id { get; set; }
    }
}
