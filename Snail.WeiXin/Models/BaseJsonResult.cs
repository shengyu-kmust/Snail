using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.WeiXin.Models
{
    /// <summary>
    /// WxJsonResult 等 Json 结果的基类（抽象类），子类必须具有不带参数的构造函数
    /// </summary>
    [Serializable]
    public abstract class BaseJsonResult : IJsonResult
    {
        /// <summary>
        /// 返回结果信息
        /// </summary>
        public virtual string errmsg { get; set; }

        /// <summary>
        /// errcode的
        /// </summary>
        public abstract int ErrorCodeValue { get; }
        public virtual object P2PData { get; set; }
    }
}
