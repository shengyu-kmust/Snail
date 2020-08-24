using Snail.Common.Extenssions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Snail.RS.Dto
{
    public class RSScheduleRuleDto : RSScheduleRule, IValidatableObject
    {
        /// <summary>
        /// 排班对象id
        /// </summary>
        [Required(ErrorMessage ="排班对象id不能为空")]
        public string TargetId { set; get; }
        /// <summary>
        /// 排班对象类型的名称，如人，业务，科室等
        /// </summary>
        [Required(ErrorMessage = "排班对象类型不能为空")]
        public string TargetType { get; set; }
        /// <summary>
        /// 排班对象的名称，如人名，业务名，科室名等
        /// </summary>
        [Required(ErrorMessage = "排班对象的名称不能为空")]
        public string TargetName { get; set; }
        /// <summary>
        /// 班次名，如上午班或下午班
        /// </summary>
        [Required(ErrorMessage = "班次名不能为空")]
        public string ScheduleName { set; get; }

        /// <summary>
        /// 排班的日期类型。按月、按星期。对应ScheduleTimeType。
        /// </summary>
        [Required(ErrorMessage = "排班的日期类型不能为空")]
        public EScheduleDayType DayType { set; get; }
        /// <summary>
        /// 日期集合。[1-7]星期，或是[1-31]日，以逗号隔开
        /// </summary>
        [Required(ErrorMessage = "排班的日期类型不能为空")]
        [RegularExpression(@"(\d,)*\d", ErrorMessage = "日期集合格式不对")]
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
        /// 是否以时间段的形式进行预约号
        /// </summary>
        public bool NumWithTime { get; set; }
        /// <summary>
        /// 不包含的日期列表，以逗号隔开。例如：20171108,20171225
        /// </summary>
        public string ExceptDate { set; get; }

        /// <summary>
        /// 包含的日期列表，以逗号隔开。例如：20171108,20171225
        /// </summary>
        public string InDate { set; get; }

        /// <summary>
        /// 扩展信息，建议存dic
        /// </summary>
        public string ExtraInfo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var error = new List<ValidationResult>();
            if (DayType==EScheduleDayType.Week && (DayList??"").Split(",").ToList().Any(a=>int.Parse(a)>7))
            {
                error.Add(new ValidationResult("当以星期的形式排班时，日期只能是1到7之间的数字"));
            }
            if (DayType == EScheduleDayType.Week && (DayList ?? "").Split(",").ToList().Any(a =>int.Parse(a) > 31))
            {
                error.Add(new ValidationResult("当以月的形式排班时，日期只能是1到31之间的数字"));
            }
            if (EndTime<=BeginTime)
            {
                error.Add(new ValidationResult("结束时间必须小于开始时间"));
            }
            if (ExceptDate.HasValue() && ExceptDate.Split(",").Any(a=>DateTime.TryParse(a,out DateTime exceptDateTemp)))
            {
                error.Add(new ValidationResult($"不包含日期参数里有不合法的日期"));
            }
            if (ExceptDate.HasValue() && InDate.Split(",").Any(a => DateTime.TryParse(a, out DateTime exceptDateTemp)))
            {
                error.Add(new ValidationResult($"包含日期参数里有不合法的日期"));
            }
            return error;
        }
    }
}
