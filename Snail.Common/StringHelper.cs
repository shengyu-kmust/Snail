using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Snail.Common
{
    public static class StringHelper
    {
        public static T ConvertTo<T>(string str)
        {
            if (str == null)
            {
                return default(T);
            }
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(str);
        }
    }
}
