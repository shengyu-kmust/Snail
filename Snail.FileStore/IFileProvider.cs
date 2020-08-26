namespace Snail.FileStore
{
    public interface IFileProvider
    {
        /// <summary>
        /// 获取文件信息的存储接口
        /// </summary>
        /// <returns></returns>
        IFileStore GetFileStore();
        /// <summary>
        /// 增加文件，除DatabaseFileProvider外，只将文件的基本信息存入到数据库
        /// </summary>
        /// <param name="fileInfo"></param>
        void Add(FileInfo fileInfo);
        /// <summary>
        /// 获取文件信息，包含文件的二进制数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FileInfo Get(string id);
    }
}
