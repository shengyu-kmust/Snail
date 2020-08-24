using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Snail.RS.Dto
{
    public class MakeAppointDto:IDto
    {
        /// <summary>
        /// 预约人id
        /// </summary>
        [Required(ErrorMessage = "预约人id必填")]
        public string SubscriberId { get; set; }
        /// <summary>
        /// 预约人名
        /// </summary>
        [Required(ErrorMessage = "预约人名必填")]
        public string SubscriberName { get; set; }

        /// <summary>
        /// 预约人联系方式
        /// </summary>
        [MaxLength(50)]
        public string SubscriberPhone { get; set; }

        [Required(ErrorMessage = "预约排班id必填")]
        public string ScheduleOfDayId { set; get; }

        /// <summary>
        /// 预约号
        /// </summary>
        [Required(ErrorMessage = "预约号必填")]
        public int OrderNum { set; get; }

        /// <summary>
        /// 扩展信息，建议存dic
        /// </summary>
        public string ExtraInfo { get; set; }

    }

    public class MakeAppointForGMDto : MakeAppointDto
    {
        /// <summary>
        /// 关联的queueid，即此queue要预约到另一天
        /// </summary>
        public List<string> QueueIds { get; set; }
        /// <summary>
        /// 预约项目描述
        /// </summary>
        public string BusinessName { get; set; }
    }
}
