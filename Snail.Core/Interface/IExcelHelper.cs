using System.Collections.Generic;
using System.IO;

namespace Snail.Core.Interface
{
    public interface IExcelHelper
    {
        List<T> ImportFromExcel<T>(Stream fileStream);
        Stream ExportToExcel<T>(List<T> data);
    }
}
