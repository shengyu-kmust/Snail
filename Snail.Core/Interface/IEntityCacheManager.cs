using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core.Interface
{
    public interface IEntityCacheManager
    {
        List<TEntity> Get<TEntity>() where TEntity : class;
        void RefreashAllEntityCache();
        void RefreshEntityCache(Type entity);
    }
}
