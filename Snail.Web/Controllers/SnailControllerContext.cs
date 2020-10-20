using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Snail.Core.Interface;
using Snail.Web;

namespace Snail.Web.Controllers
{
    /// <summary>
    /// controller公共上下文，用于定义controller类的公共方法，属性等 // todo 命名和Microsoft的有冲突
    /// </summary>
    public class SnailControllerContext
    {
        public IMapper mapper;
        public IApplicationContext applicationContext;
        public DbContext db;
        public IEntityCacheManager entityCacheManager;
        public SnailControllerContext(IMapper mapper,IApplicationContext applicationContext, DbContext db, IEntityCacheManager entityCacheManager)
        {
            this.mapper = mapper;
            this.applicationContext = applicationContext;
            this.db = db;
            this.entityCacheManager = entityCacheManager;
        }
    }
}
