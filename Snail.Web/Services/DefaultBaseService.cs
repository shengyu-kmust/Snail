namespace Snail.Web.Services
{
    public class DefaultBaseService<TEntity> : BaseService<TEntity> where TEntity : class
    {
        public DefaultBaseService(ServiceContext serviceContext) : base(serviceContext)
        {
        }
    }
}
