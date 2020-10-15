using System.ComponentModel;

namespace Snail.Core.Enum
{
    public enum EBackgroundJobType
    {
        /// <summary>
        /// 立即执行，并只执行一次
        /// </summary>
        [Description("立即执行")]
        Enqueue,
        /// <summary>
        /// 多久后执行，并执行一次
        /// </summary>
        [Description("多久后执行")]
        Schedule,
        /// <summary>
        /// 循环执行
        /// </summary>
        [Description("循环执行")]
        Recurring
    }
}
