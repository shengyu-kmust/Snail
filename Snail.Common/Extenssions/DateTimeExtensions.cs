using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Common.Extenssions
{
    public static  class DateTimeExtensions
    {
        /// <summary>
        /// 转成unix时间戳，秒
        /// </summary>
        /// <param name="localDateTime">要转的本地时间</param>
        /// <returns></returns>
        public static long ToUnixTimeSeconds(DateTime localDateTime)
        {
            return new DateTimeOffset(localDateTime).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 转成unix时间戳，毫秒
        /// </summary>
        /// <param name="localDateTime">要转的本地时间</param>
        /// <returns></returns>
        public static long ToUnixTimeMilliseconds(DateTime localDateTime)
        {
            return new DateTimeOffset(localDateTime).ToUnixTimeMilliseconds();
        }


        /// <summary>
        /// 将unix时间戳转成本地时间
        /// </summary>
        /// <param name="seconds">unix时间戳秒</param>
        /// <returns></returns>
        public static DateTime FromUnixTimeSeconds(long seconds)
        {
            return DateTimeOffset.FromUnixTimeSeconds(seconds).LocalDateTime;
        }

        /// <summary>
        /// 将unix时间戳转成本地时间
        /// </summary>
        /// <param name="milliseconds">unix时间戳毫秒</param>
        /// <returns></returns>
        public static DateTime FromUnixTimeMilliseconds(long milliseconds)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).LocalDateTime;
        }
    }
}
