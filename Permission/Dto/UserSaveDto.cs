using Snail.Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace Snail.Permission.Dto
{
    public class UserSaveDto
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "账号必填")]
        public string Account { get; set; }
        [Required(ErrorMessage = "姓名必填")]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Pwd { get; set; }
        public EGender Gender { get; set; }
    }
}
