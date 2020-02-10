using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Snail.Common.Extenssions
{
    public static class ObjectExtenssion
    {
        public static T ConvertTo<T>(this object val)
        {
            if (val.TryConvertTo<T>(out object result))
            {
                return (T)result;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 尝试类型转换
        /// </summary>
        /// <typeparam name="T">要转成的类型</typeparam>
        /// <param name="val">原值</param>
        /// <param name="toValue">转成功的值</param>
        /// <returns>是否转成功</returns>
        public static bool TryConvertTo<T>(this object val, out object toValue)
        {
            try
            {
                if (val == null)
                {
                    toValue = null;
                    return false;
                }
                if (val is T)
                {
                    toValue = val;
                    return true;
                }
                var targetType = typeof(T);
                var initialType = val.GetType();

                // 基础类型大部分都可以用Convert类来转换，注意要排除enum类型，不能将string和int通过ChangeType转成枚举
                if (typeof(IConvertible).IsAssignableFrom(targetType) && typeof(IConvertible).IsAssignableFrom(initialType))
                {
                    if (targetType.IsEnum)
                    {
                        if (val is string s)
                        {
                            toValue = Enum.Parse(targetType, s, true);
                            return true;
                        }
                        else if (val is int i)
                        {
                            toValue = Enum.ToObject(targetType, i);
                            return true;
                        }
                        else
                        {
                            toValue = null;
                            return false;
                        }
                    }
                    toValue = Convert.ChangeType(val, typeof(T));
                    return true;
                }

                // 不能用基础类型转换的，改用TypeConverter
                TypeConverter toConverter = TypeDescriptor.GetConverter(val);
                if (toConverter != null && toConverter.CanConvertTo(targetType))
                {
                    toValue = toConverter.ConvertTo(val, targetType);
                    return true;
                }

                TypeConverter fromConverter = TypeDescriptor.GetConverter(targetType);
                if (fromConverter != null && fromConverter.CanConvertFrom(initialType))
                {
                    toValue = fromConverter.ConvertFrom(val);
                    return true;
                }
                toValue = null;
                return false;
            }
            catch
            {
                toValue = null;
                return false;
            }
        }
    }
}
