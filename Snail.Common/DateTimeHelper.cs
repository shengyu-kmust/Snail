using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Common
{
    public class DateTimeHelper
    {
        public DateTime? ConvertFrom(string date)
        {
            if (DateTime.TryParse(date,out DateTime dateTime))
            {
                return dateTime;
            }
            else
            {
                return null;
            }
        }
    }
}
