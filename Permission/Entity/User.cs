using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snail.Entity
{
    public class User<TKey> : BaseEntity<TKey>
    {
        public virtual string LoginName { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Pwd { get; set; }
        public virtual int Gender { get; set; }

        #region 导航属性，下面这种导航属性用法是不正确的，会出现如下的异常。正常的做法是在外部继承类里配置导航属性
        //异常为：A key cannot be configured on 'UserTest' because it is a derived type. The key must be configured on the root type 'InnerUser<Guid>'. If you did not intend for 'InnerUser<Guid>' to be included in the model, ensure that it is not included in a DbSet property on your context, referenced in a configuration call to ModelBuilder, or referenced from a navigation property on a type that is included in the model.
        //public List<UserRole<TKey>> UserRoles { get; set; }
        //public List<UserOrg<TKey>> UserOrgs { get; set; }
        #endregion
    }
}
