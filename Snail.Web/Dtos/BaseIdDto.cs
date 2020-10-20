using Snail.Core;
using Snail.Core.Dto;

namespace Snail.Web.Dtos
{
    public class BaseIdDto : IIdField<string>, IDto
    {
        public string Id { get; set; }
    }
}
