using Snail.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Permission.Test.Entities
{
    public class UserTest:User<Guid>
    {
        public string ExtraInfo { get; set; }
    }
    public class RoleTest : InnerRole<Guid>
    {
        public string ExtraInfo { get; set; }

    }
    public class UserRole : UserRole<Guid> {
        public string ExtraInfo { get; set; }
    }
    public class Organization : Organization<Guid> {
        public string ExtraInfo { get; set; }
    }
    public class UserOrg : UserOrg<Guid> {
        public string ExtraInfo { get; set; }
        //public InnerUser<Guid> User { get; set; }

        //public UserTest User { get; set; }

    }
    public class Permission : Permission<Guid> {
        public string ExtraInfo { get; set; }
    }
    public class Resource : Resource<Guid> {
        public string ExtraInfo { get; set; }
    }
}
