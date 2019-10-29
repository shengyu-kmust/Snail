using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace Snail.Permission.Core
{
    /// <summary>
    /// 做为单例，和实体是无关的
    /// </summary>
    public class PermissionManager
    {
        private IPermissionStore _permissionStore;
        private IResourceKeyBuilder _resourceKeyBuilder;
        private ConcurrentDictionary<string, List<string>> resourceRoles;

        public PermissionManager(IPermissionStore permissionStore,IResourceKeyBuilder resourceKeyBuilder)
        {
            _permissionStore = permissionStore;
            _resourceKeyBuilder = resourceKeyBuilder;
        }

        public void LoadPermissionDatas()
        {
            throw new NotImplementedException();
        }
        
        public bool HasPermission(string userKey,string resourceKey)
        {
            throw new NotImplementedException();
        }
        public string GetResourceKey(object obj)
        {
            return _resourceKeyBuilder.BuildKey(obj);
        }
        public string GetUserKey(ClaimsPrincipal claimsPrincipal)
        {
            throw new NotImplementedException();
        }
    }
}
