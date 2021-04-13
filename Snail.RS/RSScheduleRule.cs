using Snail.Core.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snail.RS
{
    /// <summary>
    /// 排班规则，按天为单位排班，每天定时任务自动排班
    /// </summary>
    public class RSScheduleRule: DefaultBaseEntity
    {
        /// <summary>
        /// 排班对象id，如：可以是医生的id，或是科室的id等
        /// </summary>
        [MaxLength(50)]
        public string TargetId { set; get; }
        /// <summary>
        /// 排班对象类型的名称，如人，业务，科室等
        /// </summary>
        [MaxLength(50)]
        public string TargetType { get; set; }
        /// <summary>
        /// 排班对象的名称，如人名，业务名，科室名等
        /// </summary>
        [MaxLength(50)]
        public string TargetName { get; set; }
        /// <summary>
        /// 班次名，如上午班或下午班
        /// </summary>
        public string ScheduleName { set; get; }

        /// <summary>
        /// 排班的日期类型。按月、按星期。对应ScheduleTimeType。
        /// </summary>
        [MaxLength(50)]
        public EScheduleDayType DayType { set; get; }
        /// <summary>
        /// 日期集合。[0-7]星期，或是[1-31]日，以逗号隔开
        /// </summary>
        [MaxLength(100)]
        public string DayList { set; get; }
        /// <summary>
        /// 排班开始时间
        /// </summary>
        [Column(TypeName = "time")]
        public TimeSpan BeginTime { set; get; }
        /// <summary>
        /// 排班结束时间
        /// </summary>
        [Column(TypeName = "time")]
        public TimeSpan EndTime { set; get; }
        /// <summary>
        /// 总号数
        /// </summary>
        [Column(TypeName = "int")]
        public int MaxNum { set; get; }

        /// <summary>
        /// 是否以时间段的形式进行预约号
        /// </summary>
        public bool NumWithTime { get; set; }
        /// <summary>
        /// 不包含的日期列表，以逗号隔开。例如：20171108,20171225
        /// </summary>
        [MaxLength(1000)]
        public string ExceptDate { set; get; }

        /// <summary>
        /// 包含的日期列表，以逗号隔开。例如：20171108,20171225
        /// </summary>
        public string InDate { set; get; }

        /// <summary>
        /// 扩展信息，建议存dic
        /// </summary>
        public string ExtraInfo { get; set; }

    }
}