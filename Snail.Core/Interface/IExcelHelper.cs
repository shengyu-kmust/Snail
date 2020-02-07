using System;
using System.Collections.Generic;
using System.IO;

namespace Snail.Core.Interface
{
    public interface IExcelHelper
    {
        List<T> ImportFromExcel<T>(Stream fileStream);
        Stream ExportToExcel<T>(List<T> data);
    }

    public class ExcelAttribute : Attribute
    {
        /// <summary>
        /// 导出或是导入的字段对应的excel的header名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        public int Width { get; set; }
    }
}
