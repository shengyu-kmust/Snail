using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core
{
    public class BusinessException : Exception
    {
        public BusinessException(string errorMsg) : base(errorMsg)
        {

        }
    }
}
