using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.FileStore
{
    public class DatabaseFileProvider : IFileProvider
    {
        private IFileStore _fileStore;
        public DatabaseFileProvider(IFileStore fileStore)
        {
            _fileStore = fileStore;
        }
        public void Add(FileInfo fileInfo)
        {
            _fileStore.Save(fileInfo);
        }

        public FileInfo Get(string id)
        {
            return _fileStore.Get(id);
        }

        public IFileStore GetFileStore()
        {
            return _fileStore;
        }
    }
}
