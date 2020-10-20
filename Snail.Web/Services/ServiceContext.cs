using AutoMapper;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Snail.Cache;
using Snail.Core.Interface;
using System;

namespace Snail.Web.Services
{
    /// <summary>
    /// service的上下文对象，封装所有service层用到的公共或是常用的属性，方法
    /// </summary>
    public class ServiceContext
    {
        public DbContext db;
        public IEntityCacheManager entityCacheManager;
        public IMapper mapper;
        public IApplicationContext applicationContext;
        public IMemoryCache memoryCache;
        public ICapPublisher publisher;
        public IServiceProvider serviceProvider;
        public ISnailCache cache;
        public ServiceContext(DbContext db, IMapper mapper, IApplicationContext applicationContext, IEntityCacheManager entityCacheManager, IMemoryCache memoryCache, ICapPublisher publisher, IServiceProvider serviceProvider,ISnailCache cache)
        {
            this.mapper = mapper;
            this.db = db;
            this.entityCacheManager = entityCacheManager;
            this.applicationContext = applicationContext;
            this.memoryCache = memoryCache;
            this.publisher = publisher;
            this.serviceProvider = serviceProvider;
            this.cache = cache;
        }
    }
}
