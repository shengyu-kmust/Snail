using Snail.Core.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snail.RS
{
    /// <summary>
    /// 预约记录-由用户和schedule两个维度的数据组成
    /// </summary>
    public class RSRecord: DefaultBaseEntity
    {
        /// <summary>
        /// 预约人id
        /// </summary>
        [MaxLength(50)]
        public string SubscriberId { get; set; }
        /// <summary>
        /// 预约人名
        /// </summary>
        [MaxLength(50)]
        public string SubscriberName { get; set; }

        /// <summary>
        /// 预约人联系方式
        /// </summary>
        [MaxLength(50)]
        public string SubscriberPhone { get; set; }
        /// <summary>
        /// 排班ID
        /// </summary>
        [MaxLength(50)]
        public string ScheduleOfDayId { set; get; }
        /// <summary>
        /// 预约号
        /// </summary>
        [Column(TypeName = "int")]
        public int OrderNum { set; get; }

        /// <summary>
        /// 扩展信息，建议存dic
        /// </summary>
        public string ExtraInfo { get; set; }
        /// <summary>
        /// 号开始时间
        /// </summary>
        public TimeSpan NumBeginTime { get; set; }
        /// <summary>
        /// 号结束时间
        /// </summary>
        public TimeSpan NumEndTime { get; set; }

        #region 冗余
        public DateTime ScheduleDate { get; set; }
        #endregion

    }
}