using Snail.Core.Dto;
using Snail.Core.Enum;

namespace Snail.Web.Dtos
{
    public class UserResultDto: DefaultBaseDto
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public EGender Gender { get; set; }
    }
}
