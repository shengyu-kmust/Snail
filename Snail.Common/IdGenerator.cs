using System;
using System.ComponentModel;
using System.Reflection;

namespace Snail.Common
{
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
                // todo默认用snow的
                return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString($"{DateTime.Now.ToString("yyyyMMddHHmmss")}{new Random().Next(100).ToString().PadLeft(3,'0')}");
            }
            throw new NotSupportedException($"不支持此类型的id");
        }
    }
}
