using System;
using System.ComponentModel;
using System.Reflection;

namespace Snail.Common
{
    /// <summary>
    /// 自定义id生成
    /// </summary>
    public static class IdGenerator
    {
        public static TKey Generate<TKey>()
        {
            if (typeof(TKey)==typeof(Guid))
            {
                return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(Guid.NewGuid().ToString());
            }
            if (typeof(TKey) == typeof(string))
            {
                return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(SnowflakeId.Create().ToString());
            }
            if (typeof(TKey)==typeof(long))
            {
                return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(SnowflakeId.Create().ToString());
            }
            throw new NotSupportedException($"不支持此类型的id");
        }
    }
}
