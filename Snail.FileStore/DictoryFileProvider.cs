using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snail.FileStore
{
    /// <summary>
    /// 用目录的方式存储文件
    /// </summary>
    public class DictoryFileProvider : IFileProvider
    {
        private IOptionsMonitor<DictoryFileProviderOption> _optionsMonitor;
        private IFileStore _fileStore;
        public DictoryFileProvider(IOptionsMonitor<DictoryFileProviderOption> optionsMonitor,IFileStore fileStore)
        {
            _optionsMonitor = optionsMonitor;
            _fileStore = fileStore;
        }
        public void Add(FileInfo fileInfo)
        {
            var filePath = GetPath(fileInfo);
            CreateDictoryIfNotExist(filePath);
            File.WriteAllBytes(filePath, fileInfo.FileData);
            _fileStore.Save(new FileInfo
            {
                Id = fileInfo.Id,
                FileName = fileInfo.FileName,
                FileSuffix = fileInfo.FileSuffix,
                FileStoreProvider=EFileStoreProvider.Dictory,
                Length=fileInfo.Length,
                RelateDataId=fileInfo.RelateDataId,
                RelateDataType=fileInfo.RelateDataType
            });
        }

        public FileInfo Get(string id)
        {
            var fileInfo=_fileStore.Get(id);
            if (fileInfo==null)
            {
                throw new ArgumentNullException("未找到此文件");
            }
            fileInfo.FileData=File.ReadAllBytes(GetPath(fileInfo));
            return fileInfo;
        }

        public IFileStore GetFileStore()
        {
            return _fileStore;
        }

        private void CreateDictoryIfNotExist(string path)
        {
            var directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        private string GetPath(FileInfo fileInfo)
        {
            // 为避免同名文件的覆盖，文件名后加上id
            return $"{_optionsMonitor.CurrentValue.BasePath.TrimEnd(' ', '/')}/{fileInfo.FileName}_{fileInfo.Id}.{fileInfo.FileSuffix}";
        }
    }

    public class DictoryFileProviderOption
    {
        public string BasePath { get; set; }
        public int MaxLength { get; set; }
    }
}
