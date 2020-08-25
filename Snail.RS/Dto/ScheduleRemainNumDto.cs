using Snail.Core.Dto;
using System.Collections.Generic;

namespace Snail.RS.Dto
{
    public class ScheduleRemainNumDto:IDto
    {
        public List<string> Remains { get; set; }
        public int MinPerNum { get; set; }
    }
}
