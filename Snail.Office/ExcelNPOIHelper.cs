using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;
namespace Snail.Office
{
    /**
   * NPOI是从java的POI里转化过来的类库
   * NPOI.HSSF为处理excel97-2007，NPOI.XSSF处理excel2007以上的
   * NPOI.SS为
   * 
   * 
   * 
   * **/
    /// <summary>
    /// IExcelHelper人NPOI实现
    /// </summary>
    public class ExcelNPOIHelper : IExcelHelper
    {
        private readonly int _withPerChar = 256;//NPOI的每个字符的宽为256
        private ConcurrentDictionary<Type, TypeConverter> typeConverters = new ConcurrentDictionary<Type, TypeConverter>();
        public Stream ExportToExcel<T>(List<T> data, ExcelType excelType = ExcelType.XLS)
        {
            IWorkbook workbook;
            if (excelType == ExcelType.XLS)
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                workbook = new XSSFWorkbook();
            }
            var sheet = workbook.CreateSheet();
            var properties = typeof(T).GetProperties().Where(a => a.IsDefined(typeof(ExcelAttribute)))
                .OrderBy(a => a.GetCustomAttribute<ExcelAttribute>()?.Order ?? 0).ToList();
            //写入头并设置列宽
            var headerRow = sheet.CreateRow(0);
            for (int i = 0; i < properties.Count; i++)
            {
                var headCell = headerRow.CreateCell(i);
                headCell.SetCellValue(properties[i].GetCustomAttribute<ExcelAttribute>().Name);
                var columnWidth = properties[i].GetCustomAttribute<ExcelAttribute>()?.Width;
                if (columnWidth != null && columnWidth != 0)
                {
                    sheet.SetColumnWidth(i, columnWidth.Value * _withPerChar);
                }
                else
                {
                    sheet.AutoSizeColumn(i);
                }
            }

            //写入内容
            for (int i = 0; i < data.Count; i++)
            {
                var excelRow = sheet.CreateRow(i + 1);
                for (int j = 0; j < properties.Count; j++)
                {
                    var excelCell = excelRow.CreateCell(j);
                    var dataValue = properties[j].GetValue(data[i]);
                    if (dataValue != null)
                    {
                        if (dataValue is DateTime dateTime)
                        {
                            excelCell.SetCellValue(dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            excelCell.SetCellValue(properties[j].GetValue(data[i]).ToString());
                        }
                    }
                }
            }
            var memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            return memoryStream;
        }

        public List<T> ImportFromExcel<T>(Stream fileStream, int firstRowIndex = 0, int firstColumnIndex = 0) where T : new()
        {
            // 清空空的行
            var workbook = WorkbookFactory.Create(fileStream);
            var sheet = workbook.GetSheetAt(0);
            var headerRow = sheet.GetRow(firstRowIndex);
            var columnPropertyMap = new List<KeyValuePair<int, PropertyInfo>>();// T的属性和excel的列的对应关系
            var allProperties = typeof(T).GetProperties().Where(a => a.IsDefined(typeof(ExcelAttribute))).ToList();
            var result = new List<T>();
            foreach (var item in headerRow.Cells)
            {
                var property = allProperties.FirstOrDefault(a => a.GetCustomAttribute<ExcelAttribute>().Name.Equals(item.StringCellValue, StringComparison.OrdinalIgnoreCase));
                if (property != null)
                {
                    columnPropertyMap.Add(new KeyValuePair<int, PropertyInfo>(item.ColumnIndex, property));
                }
            }
            for (int i = firstRowIndex + 1; i < sheet.LastRowNum + 1; i++)
            {
                // todo 有可能每有一行的cells数不一样，cell未激活。。。。
                // todo StringCellValue会出错
                var rowObject = new T();
                var rowExcel = sheet.GetRow(i);
                foreach (var columnMap in columnPropertyMap)
                {
                    var typeConverter = typeConverters.GetOrAdd(columnMap.Value.PropertyType, type => TypeDescriptor.GetConverter(columnMap.Value.PropertyType));
                    if (typeConverter.CanConvertFrom(typeof(string)))
                    {
                        columnMap.Value.SetValue(rowObject, typeConverter.ConvertFrom(GetCellStringValue(rowExcel.GetCell(columnMap.Key))));
                    }
                }
                result.Add(rowObject);
            }
            return result;
        }

        public void ReadExcel(Stream fileStream,Action<string, int, int> action, int firstRowIndex = 0, int firstColumnIndex = 0, int rowLength = 0, int columnLength = 0)
        {
            var workbook = WorkbookFactory.Create(fileStream);
            var sheet = workbook.GetSheetAt(0);
            if (rowLength==0)
            {
                rowLength = sheet.LastRowNum+1;//lastRowNum获取的是最后一行的index值
            }
            if (columnLength==0)
            {
                var firstRow = sheet.GetRow(firstRowIndex);
                columnLength = firstRow.LastCellNum;//lastCellNum获取的是最后一列的index+1值
            }

            for (int ri = 0; ri < rowLength; ri++)
            {
                var row = sheet.GetRow(ri);
                for (int ci = 0; ci < columnLength; ci++)
                {
                    action(GetCellStringValue(row.GetCell(ci)), ri, ci);
                }
            }
        }

        private string GetCellStringValue(ICell cell)
        {
            return cell?.ToString()??"";
            //switch (cell.CellType)
            //{
            //    case CellType.Numeric:
            //        return cell.NumericCellValue.ToString();
            //    case CellType.String:
            //        return cell.StringCellValue;
            //    case CellType.Formula:
            //        return cell.CellFormula;
            //    case CellType.Boolean:
            //        return cell.BooleanCellValue.ToString();
            //    default:
            //        return "";
            //}
        }


    }
}


/*
 测试
      [HttpGet]
        public FileStreamResult GetExport()
        {
            var excelHelper = new ExcelNPOIHelper();
            var data = new List<A>
            {
                new A{Name="周晶",Age=30,DateTime=DateTime.Now,IsDelete=false},
                new A{Name="李四",Age=0,DateTime=DateTime.Now.AddDays(-10),IsDelete=true},
            };
            var stream= excelHelper.ExportToExcel(data);
            stream.Position = 0;
            return new FileStreamResult(stream, "application/vnd.ms-excel");
        }

        [HttpPost]
        public List<A> ImportExcel(IFormFile file)
        {
            var stream = file.OpenReadStream();
            var excelHelper = new ExcelNPOIHelper();
            return excelHelper.ImportFromExcel<A>(stream);
        }



     [HttpGet]
        public FileStreamResult GetExport()
        {
            var excelHelper = new ExcelNPOIHelper();
            var data = new List<A>
            {
                new A{Name="周晶",Age=30,DateTime=DateTime.Now,IsDelete=false},
                new A{Name="李四",Age=0,DateTime=DateTime.Now.AddDays(-10),IsDelete=true},
            };
            var stream= excelHelper.ExportToExcel(data);
            stream.Position = 0;
            return new FileStreamResult(stream, "application/vnd.ms-excel");
        }

        [HttpPost]
        public List<A> ImportExcel(IFormFile file)
        {
            var stream = file.OpenReadStream();
            var excelHelper = new ExcelNPOIHelper();
            return excelHelper.ImportFromExcel<A>(stream);
        }
     
     
     
     */
