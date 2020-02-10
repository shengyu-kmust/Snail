using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Common
{
    public static class ValidationHelper
    {
        public static void ArgumentNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
