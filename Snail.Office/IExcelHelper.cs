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
        /// 标准导入excel到List<T>，标准excel为列头开始位置为firstRowIndex，firstColumnIndex，列头下面全是有效数据。excel每列和T的对应由ExcelAttribute.Name控制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileStream"></param>
        /// <param name="firstRowIndex">从第几行开始，起始为0</param>
        /// <param name="firstColumnIndex">从第几列开始，起始为0</param>
        /// <returns></returns>
        List<T> ImportFromExcel<T>(Stream fileStream, int firstRowIndex = 0, int firstColumnIndex = 0) where T : new();

        /// <summary>
        /// 逐单元读取excel内容
        /// </summary>
        /// <param name="fileStream">excel流</param>
        /// <param name="firstRowIndex"></param>
        /// <param name="firstColumnIndex"></param>
        /// <param name="rowLength">为0时，将以实际计算的行为准</param>
        /// <param name="columnLength">为0时，将以实际计算的列数</param>
        /// <param name="action">每一个单元格的数据的处理委托，第一参数为cell里的值，第二个参数为rowIndex,第三个为cellIndex</param>
        void ReadExcel(Stream fileStream, Action<string, int, int> action,int firstRowIndex=0, int firstColumnIndex=0, int rowLength=0, int columnLength=0 );

        /// <summary>
        /// 导出List<T>到标准excel，标准excel为列头开始位置为第一行，列头下面全是有效数据。列头的名由T对象的ExcelAttribute.Name控制
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
