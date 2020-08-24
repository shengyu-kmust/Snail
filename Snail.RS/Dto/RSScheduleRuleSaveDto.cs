using Snail.Core.Entity;
using System;

namespace Snail.RS.Dto
{
    public class RSScheduleRuleSaveDto: IIdField<string>
    {
        public string Id { get; set; }
        /// <summary>
        /// 排班名，如上午班或下午班
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 排班人员
        /// </summary>
        public string UserId { set; get; }

        /// <summary>
        /// 排班的日期类型。按月、按星期。对应ScheduleTimeType。
        /// </summary>
        public EScheduleDayType DayType { set; get; }
        /// <summary>
        /// 日期集合。[0-7]星期，或是[1-31]日，以逗号隔开
        /// </summary>
        public string DayList { set; get; }
        /// <summary>
        /// 排班开始时间
        /// </summary>
        public TimeSpan BeginTime { set; get; }
        /// <summary>
        /// 排班结束时间
        /// </summary>
        public TimeSpan EndTime { set; get; }
        /// <summary>
        /// 总号数
        /// </summary>
        public int MaxNum { set; get; }

        /// <summary>
        /// 不包含的日期列表，以逗号隔开。例如：20171108,20171225
        /// </summary>
        public string ExceptDate { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string InDate { set; get; }
    }
}
