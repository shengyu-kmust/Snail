using System.ComponentModel.DataAnnotations.Schema;

namespace Snail.FileStore
{
    [Table("FileInfo")]
    public class FileInfo
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件后缀
        /// </summary>
        public string FileSuffix { get; set; }
        /// <summary>
        /// 文件大小，单位为byte
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// 文件数据，只有当文件是存在在数据库里时，才会有值
        /// </summary>
        public byte[] FileData { get; set; }
        /// <summary>
        /// 关联数据类型，一般为表名或是业务名，非必填
        /// </summary>
        public string RelateDataType { get; set; }
        /// <summary>
        /// 关联数据id，一般为表的id，非必填
        /// </summary>
        public string RelateDataId { get; set; }
        /// <summary>
        /// 文件存储提供程序类型
        /// </summary>
        [Column(TypeName ="varchar(20)")]
        public EFileStoreProvider FileStoreProvider { get; set; }

    }
    public enum EFileStoreProvider
    {
        Dictory,
        Database,
        Mongdb
    }
}
