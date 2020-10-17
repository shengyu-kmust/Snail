using System.ComponentModel;

namespace Snail.Core.Enum
{
    public enum EUserType
    {
        [Description("超级管理员")]
        SuperAdmin,
        [Description("管理员")]
        Admin,
        [Description("普通用户")]
        User
    }
}
