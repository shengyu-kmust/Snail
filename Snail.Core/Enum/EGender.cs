using Snail.Core.Attributes;
using System.ComponentModel;

namespace Snail.Core.Enum
{
    [EnumKeyValue]
    public enum EGender
    {
        [Description("男")]
        Male,
        [Description("女")]
        Female
    }
}
