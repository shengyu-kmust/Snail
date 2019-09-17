namespace Snail.Entity
{
    public class Resource<TKey>:BaseEntity<TKey>
    {
        /// <summary>
        /// 资源键，如接口名，菜单名
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 资源值，如url地址
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 资源描述，如接口的名称、菜单的名称
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 父资源id
        /// </summary>
        public int ParentId { get; set; }
    }
}
