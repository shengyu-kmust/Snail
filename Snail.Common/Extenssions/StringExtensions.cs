using System.ComponentModel;

namespace Snail.Common.Extenssions
{
    public static class StringExtensions
    {
        public static bool HasNotValue(this string str){
            return string.IsNullOrEmpty(str);
        }

        public static bool HasValue(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 将字符串转换成其它基础类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this string str)
        {
            if (str == null)
            {
                return default(T);
            }
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(str);
        }
    }
}
