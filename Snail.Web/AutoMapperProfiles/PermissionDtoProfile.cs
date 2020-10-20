using AutoMapper;
using Snail.Permission.Entity;
using Snail.Web.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Web.AutoMapperProfiles
{
    public class PermissionDtoProfile : Profile
    {
        public PermissionDtoProfile()
        {
            CreateMap<PermissionDefaultUser, UserResultDto>().ReverseMap();
            CreateMap<PermissionDefaultUser, UserSaveDto>().ReverseMap();
            CreateMap<PermissionDefaultRole, RoleResultDto>().ReverseMap();
            CreateMap<PermissionDefaultRole, RoleSaveDto>().ReverseMap();
            CreateMap<PermissionDefaultResource, ResourceResultDto>().ReverseMap();
            CreateMap<PermissionDefaultResource, ResourceSaveDto>().ReverseMap();
        }
    }
}
