using System;
using System.Collections.Generic;
using System.IO;

namespace Snail.Office
{
    /// <summary>
    /// excel的导出方法
    /// </summary>
    public interface IExcelHelper
    {
        /// <summary>
        /// 导入excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileStream"></param>
        /// <param name="firstRowIndex">从第几行开始</param>
        /// <param name="firstColumnIndex">从第几列开始</param>
        /// <returns></returns>
        List<T> ImportFromExcel<T>(Stream fileStream, int firstRowIndex = 0, int firstColumnIndex = 0) where T : new();
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        Stream ExportToExcel<T>(List<T> data, ExcelType excelType);

    }


    /// <summary>
    /// excel导入导出特性
    /// </summary>
    public class ExcelAttribute : Attribute
    {
        /// <summary>
        /// 导出或是导入的字段对应的excel的header名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 宽，以字符的数量的衡量，一个汉字为两字符
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 导出时的排序
        /// </summary>
        public int Order { get; set; }
    }

    public enum ExcelType
    {
        XLS,
        XLXS
    }
}
