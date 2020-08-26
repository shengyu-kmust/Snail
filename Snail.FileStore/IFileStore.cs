using System.Collections.Generic;

namespace Snail.FileStore
{
    /// <summary>
    /// 附件信息存储接口，即FileInfo的数据库存储实现
    /// </summary>
    public interface IFileStore
    {
        /// <summary>
        /// 将附件信息存入到数据里，除DatabaseFileProvider外，请不要将附件的二进制数据存储在数据库里
        /// </summary>
        /// <param name="fileInfo"></param>
        void Save(FileInfo fileInfo);
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="id"></param>
        void Delete(string id);
        /// <summary>
        /// 获取附件的详细基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FileInfo Get(string id);
        /// <summary>
        /// 获取相关数据的附件基本信息
        /// </summary>
        /// <param name="relateDataType"></param>
        /// <param name="relateDataId"></param>
        /// <returns></returns>
        List<FileInfo> GetRelateDataFileInfo(string relateDataType, string relateDataId);
    }
}
