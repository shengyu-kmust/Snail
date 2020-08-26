using Snail.Core.Dto;
using System;
using System.Collections.Generic;

namespace Snail.RS.Dto
{
    public class RSScheduleOfDayDto:IDto
    {
        public string Id { get; set; }
        #region rule相关字段
        /// <summary>
        /// 排班对象id
        /// </summary>
        public string TargetId { set; get; }
        /// <summary>
        /// 排班对象类型的名称，如人，业务，科室等
        /// </summary>
        public string TargetType { get; set; }
        /// <summary>
        /// 排班对象的名称，如人名，业务名，科室名等
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// 排班开始时间
        /// </summary>
        public TimeSpan RuleBeginTime { set; get; }
        /// <summary>
        /// 排班结束时间
        /// </summary>
        public TimeSpan RuleEndTime { set; get; }
        public int MaxNum { get; set; }

        #endregion
        /// <summary>
        /// 对应排班规则id
        /// </summary>
        public string RuleId { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 班次名，如上午班或下午班，为Rule.ScheduleName
        /// </summary>
        public string ScheduleName { set; get; }

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
        public List<RemainNumDto> RemainNumsDto { get; set; }
    }
    public class RemainNumDto
    {
        public int Num { get; set; }
        public TimeSpan TimeBegin { get; set; }
        public TimeSpan TimeEnd { get; set; }
    }
}
