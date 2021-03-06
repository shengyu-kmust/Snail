﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Core
{
    public interface IPageResult<T> : IPagination
    {
        List<T> Items { get; set; }
        int Total { get; set; }
        int TotalPage { get; }
    }
    public class PageResult<T> : IPageResult<T>
    {
        public List<T> Items { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalPage => (int)Math.Ceiling(decimal.Divide((decimal)Total, (decimal)PageSize));
    }
}
