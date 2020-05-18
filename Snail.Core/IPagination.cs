using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core
{
    public interface IPagination
    {
        int PageSize { get; set; }
        int PageIndex { get; set; }
    }

    public class BasePagination : IPagination
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
    }
}
