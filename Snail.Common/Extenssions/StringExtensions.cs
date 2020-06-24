using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

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

        public static string ToMd5(this string str)
        {
            return BitConverter.ToString(HashAlgorithm.Create(HashAlgorithmName.MD5.Name).ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", "");
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
