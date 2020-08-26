using System.ComponentModel;

namespace Snail.RS
{
    public enum EScheduleDayType
    {
        /// <summary>
        /// 按月排班，1-30
        /// </summary>
        [Description("按月排班")]
        Month, 
        /// <summary>
        /// 按周排班，1-7
        /// </summary>
        [Description("按周排班")]
        Week
    }
}
