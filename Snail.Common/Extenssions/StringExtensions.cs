using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Concurrent;

namespace Snail.Common.Extenssions
{
    public static class StringExtensions
    {
        private static readonly ConcurrentDictionary<Type, TypeConverter> typeConverters = new ConcurrentDictionary<Type, TypeConverter>();
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
            return (T)typeConverters.GetOrAdd(typeof(T), TypeDescriptor.GetConverter(typeof(T))).ConvertFromString(str);
        }
    }
}
