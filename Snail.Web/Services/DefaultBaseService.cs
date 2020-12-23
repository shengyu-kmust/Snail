using Snail.Core;

namespace Snail.Web.Services
{
    public class DefaultBaseService<TEntity,TKey> : BaseService<TEntity,TKey> 
           where TEntity : class, IIdField<TKey>
    {
        public DefaultBaseService(ServiceContext serviceContext) : base(serviceContext)
        {
        }
    }
}
