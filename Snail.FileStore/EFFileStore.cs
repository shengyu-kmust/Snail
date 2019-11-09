using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snail.FileStore
{
    public class EFFileStore : IFileStore
    {
        private DbContext _db;
        private DbSet<FileInfo> _fileInfos;
        public EFFileStore(DbContext db)
        {
            _db = db;
            _fileInfos = _db.Set<FileInfo>();
        }
        public void Delete(string id)
        {
            _fileInfos.Remove(new FileInfo { Id=id});
            _db.SaveChanges();
        }

        public FileInfo Get(string id)
        {
            return _fileInfos.AsNoTracking().FirstOrDefault(a => a.Id == id);
        }

        public List<FileInfo> GetRelateDataFileInfo(string relateDataType, string relateDataId)
        {
            var query = _fileInfos.AsNoTracking();
            if (!string.IsNullOrEmpty(relateDataType))
            {
                query = query.Where(a => a.RelateDataType == relateDataType);
            }
            if (!string.IsNullOrEmpty(relateDataId))
            {
                query = query.Where(a => a.RelateDataId == relateDataId);
            }
            return query.Select(a => new FileInfo
            {
                Id = a.Id,
                FileName = a.FileName,
                FileStoreProvider = a.FileStoreProvider,
                FileSuffix = a.FileSuffix,
                Length = a.Length,
                RelateDataId = a.RelateDataId,
                RelateDataType = a.RelateDataType
            }).ToList();
        }

        public void Save(FileInfo fileInfo)
        {
            _fileInfos.Remove(fileInfo);
            _fileInfos.Add(fileInfo);
            _db.SaveChanges();
        }
    }
}
