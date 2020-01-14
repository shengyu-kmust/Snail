using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Common.Extenssions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str){
            return string.IsNullOrEmpty(str);
        }

        public static bool HasValue(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }
    }
}
