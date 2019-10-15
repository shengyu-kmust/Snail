using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Abstract
{
    public interface IPagination
    {
        int PageSize { get; set; }
        int PageIndex { get; set; }
    }
}
