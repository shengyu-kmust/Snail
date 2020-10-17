using ApplicationCore.IServices;
using Snail.Core.Permission;
using Snail.Permission.Entity;

namespace Snail.Web.Services
{
    public class UserService : BaseService<PermissionDefaultUser >, IUserService
    {
        private IPermissionStore _permissionStore;
        public UserService(ServiceContext serviceContext,IPermissionStore permissionStore) : base(serviceContext)
        {
            _permissionStore = permissionStore;
        }
    }
}
