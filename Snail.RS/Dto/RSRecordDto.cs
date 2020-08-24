using System;
namespace Snail.RS.Dto
{
    public class RSRecordDto : IDto
    {

        /// <summary>
        /// 预约人id
        /// </summary>
        public string SubscriberId { get; set; }
        /// <summary>
        /// 预约人名
        /// </summary>
        public string SubscriberName { get; set; }

        /// <summary>
        /// 预约人联系方式
        /// </summary>
        public string SubscriberPhone { get; set; }
        /// <summary>
        /// 排班ID
        /// </summary>
        public string ScheduleOfDayId { set; get; }
        /// <summary>
        /// 预约号
        /// </summary>
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

        #region rule相关字段
        public string TargetName { get; set; }
        /// <summary>
        /// 班次名，如上午班或下午班
        /// </summary>
        public string ScheduleName { set; get; }

        /// <summary>
        /// 排班开始时间
        /// </summary>
        public TimeSpan RuleBeginTime { set; get; }
        /// <summary>
        /// 排班结束时间
        /// </summary>
        public TimeSpan RuleEndTime { set; get; }
        #endregion
    }


}
