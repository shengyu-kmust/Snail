using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core
{
    /// <summary>
    /// 业务类的异常
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string errorMsg) : base(errorMsg)
        {

        }
    }
}
