using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Snail.FileStore
{
    public class MongodbFileProvider : IFileProvider
    {

        private IOptionsMonitor<MongodbFileProviderOption> _optionsMonitor;
        private IFileStore _fileStore;
        private IMongoClient _client;
        private IMongoDatabase _database;
        private GridFSBucket _gridFSBucket;

        public MongodbFileProvider(IOptionsMonitor<MongodbFileProviderOption> optionsMonitor, IFileStore fileStore)
        {
            _optionsMonitor = optionsMonitor;
            _fileStore = fileStore;
            _client = new MongoClient(optionsMonitor.CurrentValue.ConnectString);
            _database=_client.GetDatabase(optionsMonitor.CurrentValue.DatabaseName);
            _gridFSBucket = new GridFSBucket(_database);
        }

        public IFileStore GetFileStore()
        {
            return _fileStore;
        }
        public void Add(FileInfo fileInfo)
        {
            var mongoFileId = Guid.NewGuid().ToString("N");//mongodb的ObjectId的值不能有“-”符号
            _gridFSBucket.UploadFromBytes(new ObjectId(mongoFileId), fileInfo.FileName, fileInfo.FileData,new GridFSUploadOptions { 
                Metadata=new BsonDocument(new Dictionary<string, string> { { "fileInfoId", fileInfo.Id } })
            });
            _fileStore.Save(new FileInfo
            {
                Id = fileInfo.Id,
                FileName = fileInfo.FileName,
                FileSuffix = fileInfo.FileSuffix,
                FileStoreProvider = EFileStoreProvider.Mongdb,
                Length = fileInfo.Length
            });
        }

        public FileInfo Get(string id)
        {
            var fileInfo = _fileStore.Get(id);
            var mongdbFileInfo = _gridFSBucket.Find(Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(f => f.Metadata["fileInfoId"], id))
               .FirstOrDefault();
            if (mongdbFileInfo != null)
            {
                fileInfo.FileData = _gridFSBucket.DownloadAsBytes(mongdbFileInfo.Id);
            }
            return fileInfo;
        }
    }
    public class MongodbFileProviderOption
    {
        public string ConnectString { get; set; }
        public string DatabaseName { get; set; }
    }
}
