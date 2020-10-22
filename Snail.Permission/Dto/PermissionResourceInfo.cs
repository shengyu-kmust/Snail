using Snail.Core;
using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Permission.Dto
{

    public class PermissionResourceInfo :IResource, IIdField<string>
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 资源键，如接口名，菜单名，唯一键
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 资源描述，如接口的名称、菜单的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父资源id
        /// </summary>
        public string ParentId { get; set; }

        public string GetKey()
        {
            return Id;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetParentKey()
        {
            return ParentId;
        }

        public string GetResourceCode()
        {
            return Code;
        }

        public void SetKey(string key)
        {
            this.Id = key;
        }

        public void SetName(string name)
        {
            this.Name = name;
        }

        public void SetParentKey(string parentKey)
        {
            this.ParentId = parentKey;
        }
    }
}
