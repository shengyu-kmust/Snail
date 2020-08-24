using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snail.RS
{
    /// <summary>
    /// 排班表
    /// </summary>
    [Table("RSScheduleOfDay")]
    public class RSScheduleOfDay : BaseEntity
    {
        /// <summary>
        /// 对应排班规则id
        /// </summary>
        public string RuleId { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 如上午，下午，晚上等,为ScheduleRule.Name
        /// </summary>
        public string ScheduleName { get; set; }
        /// <summary>
        /// 剩余号数
        /// </summary>
        public int RemainNum { get; set; }
        /// <summary>
        /// 剩余号源，如5，6，7，8，9，即只剩下这几个号，1-5已经被预约
        /// </summary>
        public string RemainNums { get; set; }
        /// <summary>
        /// 扩展信息，建议存dic
        /// </summary>
        public string ExtraInfo { get; set; }
    }
}
