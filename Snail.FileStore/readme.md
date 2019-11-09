# 如何用
  [Fact]
        public void DictoryFile_Test()
        {
            try
            {
                var collection = new ServiceCollection();
                collection.AddScoped<IFileProvider, DictoryFileProvider>();
                collection.AddOptions<DictoryFileProviderOption>().Configure(option => { option.BasePath = "./files"; option.MaxLength = 60000; });
                collection.AddScoped<IFileStore, EFFileStore>();
                collection.AddDbContext<DbContext,TestDbContext>(option=> {
                    option.UseSqlServer("Data Source=localhost;Initial Catalog=Test;Trusted_Connection=True;");
                });



                var services = collection.BuildServiceProvider();
                var fileProvider = services.GetService<IFileProvider>();
                var fileId = Guid.NewGuid().ToString();
                var data = File.ReadAllBytes(@"D:\2019年上半年晋级.png");
                var length = data.Length;
                fileProvider.Add(new FileInfo
                {
                    Id = fileId,
                    FileData = data,
                    FileName = "2019年上半年晋级",
                    FileSuffix = "png",
                    FileStoreProvider=EFileStoreProvider.Dictory,
                    Length=length
                });
                
                var fileInfo = fileProvider.Get(fileId);
                File.WriteAllBytes($"./{fileInfo.FileName}.{fileInfo.FileSuffix}", fileInfo.FileData);
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        [Fact]
        public void DatabaseFile_Test()
        {
            try
            {
                var collection = new ServiceCollection();
                collection.AddScoped<IFileProvider, DatabaseFileProvider>();
                collection.AddScoped<IFileStore, EFFileStore>();
                collection.AddDbContext<DbContext, TestDbContext>(option => {
                    option.UseSqlServer("Data Source=localhost;Initial Catalog=Test;Trusted_Connection=True;");
                });
                var services = collection.BuildServiceProvider();
                var fileProvider = services.GetService<IFileProvider>();
                var fileId = Guid.NewGuid().ToString();
                var data = File.ReadAllBytes(@"D:\2019年上半年晋级.png");
                var length = data.Length;
                fileProvider.Add(new FileInfo
                {
                    Id = fileId,
                    FileData = data,
                    FileName = "2019年上半年晋级",
                    FileSuffix = "png",
                    Length= length,
                    FileStoreProvider=EFileStoreProvider.Database
                });
                var fileInfo = fileProvider.Get(fileId);
                File.WriteAllBytes($"./{fileInfo.FileName}_{fileInfo.Id}.{fileInfo.FileSuffix}", fileInfo.FileData);
            }
            catch (Exception ex)
            {
                var s = ex;
            }
        }

        [Fact]
        public void MongodbFile_Test()
        {
            try
            {
                var collection = new ServiceCollection();
                collection.AddScoped<IFileProvider, MongodbFileProvider>();
                collection.AddScoped<IFileStore, EFFileStore>();
                collection.AddOptions<MongodbFileProviderOption>().Configure(option => { option.ConnectString = "mongodb://localhost/admin"; option.DatabaseName = "admin"; });
                collection.AddDbContext<DbContext, TestDbContext>(option => {
                    option.UseSqlServer("Data Source=localhost;Initial Catalog=Test;Trusted_Connection=True;");
                });
                var services = collection.BuildServiceProvider();
                var fileProvider = services.GetService<IFileProvider>();
                var fileId = Guid.NewGuid().ToString();
                var data = File.ReadAllBytes(@"D:\2019年上半年晋级.png");
                var length = data.Length;
                fileProvider.Add(new FileInfo
                {
                    Id = fileId,
                    FileData = data,
                    FileName = "2019年上半年晋级",
                    FileSuffix = "png",
                    Length = length,
                    FileStoreProvider = EFileStoreProvider.Database
                });
                var fileInfo = fileProvider.Get(fileId);
                File.WriteAllBytes($"./{fileInfo.FileName}_{fileInfo.Id}.{fileInfo.FileSuffix}", fileInfo.FileData);
            }
            catch (Exception ex)
            {
                var s = ex;
            }
        }