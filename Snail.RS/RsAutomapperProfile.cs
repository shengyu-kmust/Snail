using AutoMapper;
using Snail.Core.Dto;
using Snail.Core.Entity;
using Snail.RS.Dto;
using System.Linq;

namespace Snail.RS
{
    public class RsAutomapperProfile : Profile
    {
        public RsAutomapperProfile()
        {
            MapEntityAndDto();
        }

        private void MapEntityAndDto()
        {
            var allEntities = typeof(RSRecord).Assembly.DefinedTypes.ToList().Where(a => a.GetInterfaces().Any(i => i == typeof(IEntityId<string>))).ToList();
            var allDtos = typeof(RSRecord).Assembly.DefinedTypes.ToList().Where(a => a.GetInterfaces().Any(i => i == typeof(IDto))).ToList();
            allEntities.ForEach(entity =>
            {
                allDtos.Where(a => a.Name.StartsWith(entity.Name)).ToList().ForEach(dto =>
                {
                    CreateMap(entity, dto);
                    CreateMap(dto, entity);
                });
            });
        }
    }
}
