using Snail.Core.Entity;

namespace Snail.Core.Dto
{
    public class DefaultBaseDto : IDto, IIdField<string>
    {
        public string Id { get; set; }
    }
}
