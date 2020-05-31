using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace Snail.Common
{
    public static class EnumHelper
    {
        private static ConcurrentDictionary<Type, ConcurrentDictionary<string, string>> _enumDescription = new ConcurrentDictionary<Type, ConcurrentDictionary<string, string>>();
        public static string GetEnumDescription(this Enum val)
        {
            if (!_enumDescription.ContainsKey(val.GetType()))
            {
                Func<Type, ConcurrentDictionary<string, string>> getDic = (type) =>
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
                };

                _enumDescription.TryAdd(val.GetType(), getDic(val.GetType()));
            }

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

    }
}
