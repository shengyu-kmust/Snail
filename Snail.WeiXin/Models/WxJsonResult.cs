using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.WeiXin.Models
{

    /// <summary>
    /// 包含 errorcode 的 Json 返回结果接口
    /// </summary>
    public interface IWxJsonResult : IJsonResult
    {
        /// <summary>
        /// 返回结果代码
        /// </summary>
        ReturnCode errcode { get; set; }
    }

    /// <summary>
    /// 公众号 JSON 返回结果（用于菜单接口等），子类必须具有不带参数的构造函数
    /// </summary>
    [Serializable]
    public class WxJsonResult : BaseJsonResult
    {
        //会造成循环引用
        //public WxJsonResult BaseResult
        //{
        //    get { return this; }
        //}

        public ReturnCode errcode { get; set; }

        /// <summary>
        /// 返回消息代码数字（同errcode枚举值）
        /// </summary>
        public override int ErrorCodeValue { get { return (int)errcode; } }

        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public WxJsonResult() { }


        public override string ToString()
        {
            return string.Format("WxJsonResult：{{errcode:'{0}',errcode_name:'{1}',errmsg:'{2}'}}",
                (int)errcode, errcode.ToString(), errmsg);
        }
    }

}
