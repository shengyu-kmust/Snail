using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
            var properties = typeof(T).GetProperties().ToList().Where(a => a.IsDefined(typeof(ExcelAttribute)))
                .OrderBy(a => a.GetCustomAttribute<ExcelAttribute>()?.Order ?? 0).ToList();
            //写入头并设置列宽
            var headerRow = sheet.CreateRow(0);
            for (int i = 0; i < properties.Count(); i++)
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
                for (int j = 0; j < properties.Count(); j++)
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
            var columnPropertyMap = new List<KeyValuePair<int, PropertyInfo>>();
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
                    var typeConverter = TypeDescriptor.GetConverter(columnMap.Value.PropertyType);
                    if (typeConverter.CanConvertFrom(typeof(string))) // todo 类型转换
                    {
                        try
                        {
                            columnMap.Value.SetValue(rowObject, typeConverter.ConvertFrom(rowExcel.Cells[columnMap.Key].StringCellValue));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                result.Add(rowObject);
            }
            return result;
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
