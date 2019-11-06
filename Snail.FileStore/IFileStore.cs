using System.Collections.Generic;

namespace Snail.FileStore
{
    public interface IFileStore
    {
        void Save(FileInfo fileInfo);
        void Delete(string id);
        FileInfo Get(string id);
        List<FileInfo> GetRelateDataFileInfo(string relateDataType, string relateDataId);
    }
}
