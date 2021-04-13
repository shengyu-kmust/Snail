using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace Snail.Common
{
    /// <summary>
    /// EnumHelper
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 存储enum相关信息，key=type,value={key=fieldName,value=description}
        /// </summary>
        private static ConcurrentDictionary<Type, ConcurrentDictionary<string, string>> _enumDescription = new ConcurrentDictionary<Type, ConcurrentDictionary<string, string>>();

        /// <summary>
        /// 获取enum的description
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum val)
        {
            EnsureEnum(val.GetType());

            if (_enumDescription.TryGetValue(val.GetType(), out ConcurrentDictionary<string, string> dics))
            {
                if (dics.TryGetValue(val.ToString(), out string result))
                {
                    return result;
                }
                else
                {
                    return "";
                }
            }
            else { return ""; }

        }

        /// <summary>
        /// 将des传成enum类型
        /// </summary>
        /// <param name="enumType">enum的类型</param>
        /// <param name="des">des</param>
        /// <param name="enum">enum</param>
        /// <returns>true:能转成功，false：转失败</returns>
        public static bool TryConvertDesToEnum(Type enumType,string des,out object @enum)
        {
            //@enum= enumType.IsValueType ? Activator.CreateInstance(enumType) : null;
            var result = false;
            @enum = default;
            EnsureEnum(enumType);
            if (_enumDescription.TryGetValue(enumType, out ConcurrentDictionary<string, string> dics))
            {
                foreach (var item in dics)
                {
                    if (string.Equals(item.Value,des,StringComparison.OrdinalIgnoreCase))
                    {
                        @enum = Enum.Parse(enumType, item.Key);
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        public static void EnsureEnum(Type enumType)
        {
            if (!_enumDescription.ContainsKey(enumType))
            {
                _enumDescription.TryAdd(enumType, GetEnumDic(enumType));
            }
        }

        /// <summary>
        /// 返回enum的fieldName-description字典
        /// </summary>
        public static ConcurrentDictionary<string, string> GetEnumDic(Type type)
        {
            var addDic = new ConcurrentDictionary<string, string>();
            foreach (var item in type.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                var attr = Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute));
                if (attr != null)
                {
                    addDic.TryAdd(item.Name, ((DescriptionAttribute)attr).Description);
                }
            }
            return addDic;
        }

        

    }
}
