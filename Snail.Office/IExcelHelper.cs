using System.Collections.Generic;
using System.IO;

namespace Snail.Office
{
    public interface IExcelHelper
    {
        List<T> ImportFromExcel<T>(Stream fileStream);
    }
}
