using Snail.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Permission.Test
{
    public class TestUser:User<Guid>{}
    public class TestRole : Role<Guid>{}
    public class TestUserRole : UserRole<Guid>{}
    public class TestOrganization : Organization<Guid>{}
    public class TestUserOrg : UserOrg<Guid>{}
    public class TestResource : Resource<Guid>{ }
    public class TestPermission : Permission<Guid>{ }

}
