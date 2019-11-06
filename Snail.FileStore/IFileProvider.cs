using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.FileStore
{
    public interface IFileProvider
    {
        IFileStore GetFileStore();
        void Add(FileInfo fileInfo);
        FileInfo Get(string id);
    }
}
