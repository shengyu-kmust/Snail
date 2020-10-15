using Snail.Core.Dto;
using Snail.Core.Entity;

namespace Snail.Web.Dtos
{
    public class BaseQueryPaginationDto : Pagination, IIdField<string>, IDto
    {
        public string Id { get;set;}
    }
}
