using AutoMapper;
using Snail.Core.Dto;
using Snail.Core.Entity;
using Snail.Web.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Web.AutoMapperProfiles
{
    /// <summary>
    /// 默认的automapper配置，只配置entity和dto的相互映射，其它的请用AutoMapAttribute设置
    /// </summary>
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            MapEntityAndDto();
        }

        private void MapEntityAndDto()
        {
            var assemblies = new List<Assembly>
            {
                Assembly.Load("Snail.Web"),
                Assembly.Load("Snail.Permission"),

            };
            var allEntities = assemblies.Select(a => a.DefinedTypes).SelectMany(a => a)
                .Where(a => a.GetInterfaces().Any(i => i == typeof(IEntityId<string>))).ToList();
            var allDtos = assemblies.Select(a => a.DefinedTypes).SelectMany(a => a)
                .Where(a => a.GetInterfaces().Any(i => i == typeof(IDto))).ToList();

            allEntities.ForEach(entity =>
            {
                allDtos.Where(a => a.Name.StartsWith(entity.Name)).ToList().ForEach(dto =>
                {
                    CreateMap(entity, dto);
                    CreateMap(dto, entity);
                });
            });

            // 自身的映射
            allDtos.ForEach(dto =>
            {
                CreateMap(dto, dto);
            });
            allEntities.ForEach(entity =>
            {
                CreateMap(entity, entity);

            });
        }
    }
}
