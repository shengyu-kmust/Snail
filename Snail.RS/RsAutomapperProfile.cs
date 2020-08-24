using AutoMapper;
using Snail.Core.Entity;
using Snail.RS.Dto;
using System;
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
            var allEntities = typeof(BaseEntity).Assembly.DefinedTypes.ToList().Where(a => a.GetInterfaces().Any(i => i == typeof(IEntityId<string>))).ToList();
            var allDtos = typeof(IDto).Assembly.DefinedTypes.ToList().Where(a => a.GetInterfaces().Any(i => i == typeof(IDto))).ToList();
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
