using System.ComponentModel;

namespace Snail.Core.Enum
{
    public enum EDatabaseType
    {
        [Description("SqlServer")]
        SqlServer,
        [Description("MySql")]
        MySql,
        [Description("Oracle")]
        Oracle
    }
}
