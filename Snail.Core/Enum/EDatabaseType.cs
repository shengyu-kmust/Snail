﻿using Snail.Core.Attributes;
using System.ComponentModel;

namespace Snail.Core.Enum
{
    [EnumKeyValue]
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
