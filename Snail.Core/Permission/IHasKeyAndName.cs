namespace Snail.Core.Permission
{
    public interface IHasKeyAndName
    {
        /// <summary>
        /// 一般为id，主键
        /// </summary>
        /// <returns></returns>
        string GetKey();
        /// <summary>
        /// 一般为描述
        /// </summary>
        /// <returns></returns>
        string GetName();
        /// <summary>
        /// 设置名
        /// </summary>
        void SetName(string name);
        void SetKey(string key);
    }
}
