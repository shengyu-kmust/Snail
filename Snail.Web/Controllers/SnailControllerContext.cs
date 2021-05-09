using AutoMapper;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Snail.Cache;
using Snail.Core.Default.Service;
using Snail.Core.Interface;
using Snail.Web;
using Snail.Web.Services;
using System;

namespace Snail.Web.Controllers
{
    /// <summary>
    /// controller公共上下文，用于定义controller类的公共方法，属性等 
    /// </summary>
    public class SnailControllerContext
    {
        public IMapper mapper;
        public IApplicationContext applicationContext;
        public DbContext db;
        public ServiceContext serviceContext;
        public IEntityCacheManager entityCacheManager;
        public IMemoryCache memoryCache;
        public ICapPublisher publisher;
        public IServiceProvider serviceProvider;
        public ISnailCache cache;
        public SnailControllerContext(IMapper mapper,IApplicationContext applicationContext, DbContext db, IEntityCacheManager entityCacheManager, ServiceContext serviceContext, IMemoryCache memoryCache, ICapPublisher publisher, IServiceProvider serviceProvider, ISnailCache cache)
        {
            this.mapper = mapper;
            this.applicationContext = applicationContext;
            this.db = db;
            this.entityCacheManager = entityCacheManager;
            this.serviceContext = serviceContext;
            this.serviceProvider = serviceProvider;
            this.cache = cache;
            this.memoryCache = memoryCache;
        }
    }
}
