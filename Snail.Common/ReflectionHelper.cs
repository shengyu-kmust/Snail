using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Common
{
    public static class ReflectionHelper
    {

        /// <summary>
        /// 判断类型是否为可空
        /// </summary>
        /// <param name="t">要判断的类型</param>
        /// <returns>true,false</returns>
        public static bool IsNullableType(Type t)
        {
            ValidationHelper.ArgumentNotNull(t, nameof(t));
            return (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

    }
}
